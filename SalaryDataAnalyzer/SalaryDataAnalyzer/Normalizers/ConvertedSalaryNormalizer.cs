using System;
using System.Collections.Generic;

namespace SalaryDataAnalyzer.Contracts
{
    public class ConvertedSalaryNormalizer : ResponseNormalizerBase
    {
        private const decimal MaxSalary = 1_000_000;
        private const decimal MinSalary = 5_000;
        public override string HeaderValue => "ConvertedSalary";

        protected override IDictionary<string, decimal> ResponseScale
            => throw new NotSupportedException();

        public override decimal? NormalizeData(string rawData)
        {
            if (decimal.TryParse(rawData, out var value))
            {
                return value > MaxSalary || value < MinSalary
                    ? new decimal?()
                    : value / MaxSalary;
            }

            return null;
        }
    }
}
