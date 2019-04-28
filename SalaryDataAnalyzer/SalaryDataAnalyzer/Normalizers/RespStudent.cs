
namespace SalaryDataAnalyzer.Contracts
{
    class RespStudent
    {
        public double Value { get; set; }
        public static double Normalize(string rawData)
        {
            double numericValue = 0;
            string[] options =
            {
                "No",
                "Yes, part-time",
                "Yes, full-time"
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
