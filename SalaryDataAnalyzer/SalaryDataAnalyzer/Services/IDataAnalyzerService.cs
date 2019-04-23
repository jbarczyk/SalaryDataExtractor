using System.Collections.Generic;
using SalaryDataAnalyzer.Contracts;

namespace SalaryDataAnalyzer.Services
{
    public interface IDataAnalyzerService
    {
        int GetCountOfFullAnswers(Survey survey, IEnumerable<string> headers);
    }
}