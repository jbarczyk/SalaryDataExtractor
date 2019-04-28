
namespace SalaryDataAnalyzer.Contracts
{
    class RespYearsCoding
    {
        public double Value { get; set; }
        public static double Normalize(string rawData)
        {
            double numericValue = 0;
            string[] options = 
            {
                "0-2 years",
                "3-5 years",
                "6-8 years",
                "9-11 years",
                "12-14 years",
                "15-17 years",
                "18-20 years",
                "21-23 years",
                "24-26 years",
                "27-29 years",
                "30 or more years"
            };

            int index = 1;
            foreach (string option in options)
            {
                if (option == rawData)
                {
                    numericValue = index;
                    break;
                }
                index++;
            }

            //standardization
            numericValue /= options.Length; 

            return numericValue;
        }
    }
}
