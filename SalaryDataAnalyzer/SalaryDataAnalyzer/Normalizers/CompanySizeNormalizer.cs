using System.Collections.Generic;

namespace SalaryDataAnalyzer.Contracts
{
    public class CompanySizeNormalizer : ResponseNormalizerBase
    {
        public override string HeaderValue => "CompanySize";
        protected override IDictionary<string, decimal> ResponseScale
            => new Dictionary<string, decimal>
            {
                { "Fewer than 10 employees", 1m },
                { "10 to 19 employees", 2m },
                { "20 to 99 employees", 3m },
                { "100 to 499 employees", 4m },
                { "500 to 999 employees", 5m },
                { "1,000 to 4,999 employees", 6m },
                { "5,000 to 9,999 employees", 7m },
                { "10,000 or more employees", 8m }
            };
    }
}