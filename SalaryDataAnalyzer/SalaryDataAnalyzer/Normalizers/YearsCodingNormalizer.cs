using System.Collections.Generic;

namespace SalaryDataAnalyzer.Contracts
{
    class YearsCodingNormalizer : ResponseNormalizerBase
    {
        public override string HeaderValue => "FormalEducation";
        protected override IDictionary<string, decimal> ResponseScale
            => new Dictionary<string, decimal>
        {
            { "0-2 years", 0m },
            { "3-5 years", 3m },
            { "6-8 years", 6m },
            { "9-11 years", 9m },
            { "12-14 years", 12m },
            { "15-17 years", 15m },
            { "18-20 years", 18m },
            { "21-23 years", 21m },
            { "24-26 years", 24m },
            { "27-29 years", 27m },
            { "30 or more years", 30m }
        };
    }
}
