using System.Collections.Generic;
using System.Linq;

namespace SalaryDataAnalyzer.Contracts
{
    public abstract class ResponseNormalizerBase
    {
        public abstract string HeaderValue { get; }
        protected abstract IDictionary<string, decimal> ResponseScale { get; }
        public virtual decimal? NormalizeData(string rawData)
        {
            return ResponseScale.TryGetValue(rawData, out var value)
                ? value / ResponseScale.Max(x => x.Value)
                : new decimal?();
        }
    }
}
