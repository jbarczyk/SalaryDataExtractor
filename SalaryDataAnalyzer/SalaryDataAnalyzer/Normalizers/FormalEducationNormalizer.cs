using System.Collections.Generic;

namespace SalaryDataAnalyzer.Contracts
{
    class FormalEducationNormalizer : ResponseNormalizerBase
    {
        public override string HeaderValue => "FormalEducation";
        protected override IDictionary<string, decimal> ResponseScale
            => new Dictionary<string, decimal>
            {
                { "I never completed any formal education", 1m },
                { "Primary/elementary school", 2m },
                { "Secondary school (e.g. American high school, German Realschule or Gymnasium, etc.)", 3m },
                { "Some college/university study without earning a degree", 5m },
                { "Associate degree", 8m },
                { "Bachelor’s degree (BA, BS, B.Eng., etc.)", 13m },
                { "Master’s degree (MA, MS, M.Eng., MBA, etc.)", 21m },
                { "Other doctoral degree (Ph.D, Ed.D., etc.)", 34m },
                { "Professional degree (JD, MD, etc.)", 69m }
            };
    }
}
