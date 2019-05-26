
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalaryDataAnalyzer.Contracts
{
    public class GenericDevTypeNormalizer : ResponseNormalizerBase
    {
        private readonly string _currentDevType;
        public override string HeaderValue => "DevType";
        protected override IDictionary<string, decimal> ResponseScale
            => throw new NotSupportedException();

        public GenericDevTypeNormalizer(string devType)
        {
            _currentDevType = devType;
        }

        private readonly IDictionary<string, int> _devTypes = new Dictionary<string, int>
        {
            { "Back-end developer", 1 },
            { "C-suite executive (CEO, CTO, etc.)", 2 },
            { "Data or business analyst", 4 },
            { "Data scientist or machine learning specialist", 8 },
            { "Database administrator", 16 },
            { "Designer", 32 },
            { "Desktop or enterprise applications developer", 64 },
            { "DevOps specialist", 128 },
            { "Educator or academic researcher", 256 },
            { "Embedded applications or devices developer", 512 },
            { "Engineering manager", 1024 },
            { "Front-end developer", 2048 },
            { "Full-stack developer", 4096 },
            { "Game or graphics developer", 8192 },
            { "Marketing or sales professional", 16384 },
            { "Mobile developer", 32768 },
            { "Product manager", 65536 },
            { "QA or test developer", 131072 },
            { "Student", 262144 },
            { "System administrator", 524288 }
        };

        public override decimal? NormalizeData(string rawData)
        {
            //DevType has many values sorted from the most important
            var separated = rawData.Split(';');

            var type = _devTypes
                .Where(x => separated.Contains(x.Key))
                .Select(x => x.Value)
                .First();

            return _currentDevType.Equals(type)
                ? 1m
                : 0m;
        }
    }
}
