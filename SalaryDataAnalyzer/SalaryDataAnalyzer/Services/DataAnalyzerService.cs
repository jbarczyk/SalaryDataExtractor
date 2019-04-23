using System.Collections.Generic;
using System.Linq;
using SalaryDataAnalyzer.Contracts;

namespace SalaryDataAnalyzer.Services
{
    public class DataAnalyzerService : IDataAnalyzerService
    {
        private const string NotAnswered = "NA";
        public int GetCountOfFullAnswers(
            Survey survey, IEnumerable<string> headers)
        {
            var indexes = survey.Questions
                .Select((question, i) => new { question, i })
                .Where(x => headers.Contains(x.question.Header))
                .Select(x => x.i);

            return survey.Responses.Count(x =>
                indexes.All(i => !string.IsNullOrWhiteSpace(x.Answers[i]) && !x.Answers[i].Equals(NotAnswered)));
        }
    }
}
