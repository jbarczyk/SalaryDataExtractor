using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LumenWorks.Framework.IO.Csv;
using SalaryDataAnalyzer.Contracts;

namespace SalaryDataAnalyzer.Extractors
{
    public class CsvExtractor : ICsvExtractor
    {
        private readonly AppSettings _settings;

        public CsvExtractor()
        {
            _settings = new AppSettings
            {
                SeperatorCsv = ',',
                CsvFilePath = "Data/survey_results_public.csv",
                CsvSchemaFilePath = "Data/survey_results_schema.csv",
                CsvCountriesFilePath = "Data/countries.csv"
            };
        }

        public async Task<IEnumerable<Question>> GetHeaders()
        {
            long id = 1;
            var result = new List<Question>();
            using (var streamReader = new StreamReader(_settings.CsvSchemaFilePath))
            {
                var line = await streamReader.ReadLineAsync();
                while (!streamReader.EndOfStream)
                {
                    line = await streamReader.ReadLineAsync();
                    var index = line.IndexOf(_settings.SeperatorCsv);
                    var question = index == -1
                        ? new Question { Id = id, Header = line }
                        : new Question
                        {
                            Id = id,
                            Header = line.Substring(0, index),
                            Description = line.Substring(index + 1)
                        };

                    result.Add(question);
                    ++id;
                }
            }

            return result;
        }

        public Survey GetSurvey()
        {
            var result = new List<Responses>();

            using (var csv = new CsvReader(new StreamReader(_settings.CsvFilePath), true))
            {
                var fieldCount = csv.FieldCount;
                var headers = csv.GetFieldHeaders();
                while (csv.ReadNextRecord())
                {
                    result.Add(new Responses
                    {
                        Answers = Enumerable.Range(0, fieldCount).Select(i => csv[i]).ToArray()
                    });
                }

                return new Survey
                {
                    Questions = headers.Select(x => new Question { Header = x }),
                    Responses = result
                };
            }
        }


        public List<string> GetCountries()
        {
            var result = new List<string>();

            using (var reader = new StreamReader(_settings.CsvCountriesFilePath))
            {
                var content = reader.ReadLine();
                var array = content.Split(',');
                result.AddRange(array);
            }
            return result;
        }
    }
}
