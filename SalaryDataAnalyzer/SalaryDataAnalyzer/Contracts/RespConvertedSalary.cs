
using System;

namespace SalaryDataAnalyzer.Contracts
{
    class RespConvertedSalary
    {
        public double Value { get; set; }
        public static double Normalize(string rawData)
        {
            double numericValue = 0;

            if (rawData != null && rawData != "NA")
            { 
            numericValue = Math.Max(5000, Math.Min(1000000, int.Parse(rawData)));
            } 

            //standardization
            numericValue /= 1000000;

            return numericValue;
        }
    }
}
