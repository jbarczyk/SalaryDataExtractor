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

        private readonly List<List<string>> _coutryBrackets = new List<List<string>>()
        {
            new List<string>(){ "Republic of Korea", "Bolivia", "Lebanon", "Panama", "Dominican Republic", "Zimbabwe", "Ethiopia", "Georgia", "Uzbekistan", "Venezuela", "Belarus", "Slovakia", "Malawi", "Uruguay", "Jordan", "South Korea", "Turkey" },
            new List<string>(){ "Morocco", "Austria", "Thailand", "Croatia", "Argentina", "China", "Benin", "Sweden", "Nigeria", "Philippines" , "United Arab Emirates", "Sri Lanka", "Luxembourg","Bosnia and Herzegovina", "Ireland", "Poland", "Canada"},
            new List<string>(){ "New Zealand","Saudi Arabia","Belgium","Libyan Arab Jamahiriya","Germany","South Africa","Japan","Myanmar","Uganda","India","Romania","Ukraine","United Kingdom","Iceland","Italy","Brazil","Australia" },
            new List<string>(){ "United States","Nepal","Colombia","France","Bulgaria","Algeria","Switzerland","Kuwait","Norway","Czech Republic","Bangladesh","Denmark","Indonesia","Nicaragua","Honduras","Ecuador","Israel"},
            new List<string>(){ "Hungary","Spain","Angola","Mexico","Serbia","Portugal","Egypt","Gabon","Cameroon","Netherlands","Iraq","Greece","Afghanistan","Cyprus","Mongolia","Russian Federation","Namibia"},
            new List<string>(){ "Lithuania","Taiwan","Estonia","Singapore","Bahrain","Kenya","Iran","Pakistan","Trinidad and Tobago","Finland","Tajikistan","Democratic Republic of the Congo","Other Country (Not Listed Above)","Slovenia","Malaysia","Albania","Peru"},
            new List<string>(){ "Viet Nam","Malta","Costa Rica","Azerbaijan","Chile","Tunisia","Hong Kong (S.A.R.)","Madagascar","Paraguay","Latvia","Ghana","Republic of Moldova","Cuba","El Salvador","Kyrgyzstan","Guatemala","Maldives"},
            new List<string>(){ "Turkmenistan","Mozambique","Kazakhstan","Mauritius","The former Yugoslav Republic of Macedonia","United Republic of Tanzania","Armenia","Syrian Arab Republic","Somalia","Andorra","Montenegro","Rwanda","Cambodia","Jamaica","Sudan","Oman","Bhutan" }
        };

        public override decimal? NormalizeData(string rawData)
        {
            if (rawData.ToUpper().Equals("NA"))
            {
                return 0m;
            }

            return _currentCountries.Any(x => x.Equals(rawData))
                ? 1m
                : 0m;
        }
    }
}
