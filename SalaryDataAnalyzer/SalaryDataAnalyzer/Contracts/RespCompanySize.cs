
namespace SalaryDataAnalyzer.Contracts
{
    class RespCompanySize
    {
        public double Value { get; set; }
        public static double Normalize(string rawData)
        {
            double numericValue = 0;
            string[] options =
            {
                "Fewer than 10 employees",
                "10 to 19 employees",
                "20 to 99 employees",
                "100 to 499 employees",
                "500 to 999 employees",
                "1,000 to 4,999 employees",
                "5,000 to 9,999 employees",
                "10,000 or more employees"
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
