using System.Collections.Generic;
using System.Threading.Tasks;
using SalaryDataAnalyzer.Contracts;

namespace SalaryDataAnalyzer.Extractors
{
    public interface ICsvExtractor
    {
        Task<IEnumerable<Question>> GetHeaders();
        Survey GetSurvey();
    }
}