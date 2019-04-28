
using System.Collections.Generic;

namespace SalaryDataAnalyzer.Contracts
{
    class EmploymentNormalizer : ResponseNormalizerBase
    {
        public double Value { get; set; }

        public override string HeaderValue => "Employment";

        protected override IDictionary<string, decimal> ResponseScale
            => new Dictionary<string, decimal>
            {
                { "Not employed, and not looking for work", 1m },
                { "Not employed, but looking for work", 2m },
                { "Independent contractor, freelancer, or self-employed", 4m },
                { "Employed part-time", 6m },
                { "Employed full-time", 10m },
                { "Retired", 3m }
            };
    }
}
