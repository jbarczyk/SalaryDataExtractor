using System.Collections.Generic;
using System.Linq;
using SalaryDataAnalyzer.Extractors;

namespace SalaryDataAnalyzer.Contracts
{
    public class CountryNormalizer : ResponseNormalizerBase
    {
        public override string HeaderValue => "Country";
        protected override IDictionary<string, decimal> ResponseScale
            => GetCountries();

        private static IDictionary<string, decimal> GetCountries()
        {
            var extractor = new CsvExtractor();
            var countries = extractor.GetCountries();
            return countries
                .Select((country, order) => new { country, order })
                .ToDictionary(x => x.country, x => (decimal)x.order);
        }
    }
}
