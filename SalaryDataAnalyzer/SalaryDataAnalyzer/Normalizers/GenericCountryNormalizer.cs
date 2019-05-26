using System.Collections.Generic;
using System.Linq;

namespace SalaryDataAnalyzer.Contracts
{
    public class GenericCountryNormalizer : ResponseNormalizerBase
    {
        private readonly IEnumerable<string> _currentCountries;
        public override string HeaderValue => "Country";

        protected override IDictionary<string, decimal> ResponseScale 
            => throw new System.NotImplementedException();

        public GenericCountryNormalizer(IEnumerable<string> countries)
        {
            _currentCountries = countries;
        }

        public override decimal? NormalizeData(string rawData)
        {
            return _currentCountries.Contains(rawData)
                ? 1m
                : 0m;
        }
    }
}
