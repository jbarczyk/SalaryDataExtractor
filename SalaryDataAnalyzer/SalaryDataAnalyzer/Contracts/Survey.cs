using System.Collections.Generic;

namespace SalaryDataAnalyzer.Contracts
{
    public class Survey
    {
        public IEnumerable<Question> Questions { get; set; }
        public IEnumerable<Responses> Responses { get; set; }
    }
}
