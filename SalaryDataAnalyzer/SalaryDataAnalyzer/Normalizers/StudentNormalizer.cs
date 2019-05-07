using System.Collections.Generic;

namespace SalaryDataAnalyzer.Contracts
{
    class StudentNormalizer : ResponseNormalizerBase
    {
        public override string HeaderValue => "FormalEducation";
        protected override IDictionary<string, decimal> ResponseScale
            => new Dictionary<string, decimal>
            {
            { "No", 0m },
            { "Yes, part-time", 1m },
            { "Yes, full-time", 2m }
        };
    }
}
