
using SalaryDataAnalyzer.Extractors;

namespace SalaryDataAnalyzer.Contracts
{
    class RespCountry
    {
        public double Value { get; set; }
        public static double Normalize(string rawData)
        {
            CsvExtractor _extractor = new CsvExtractor();
            double numericValue = 0;

            var countries = _extractor.GetCountries();

            int index = 1;
            foreach(string country in countries)
            {
                if(country == rawData)
                {
                    numericValue = index;
                    break;
                }
                index++;
            }

            //standardization
            numericValue /= countries.Count;

            return numericValue;
        }
    }
}
