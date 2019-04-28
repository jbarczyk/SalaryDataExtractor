
namespace SalaryDataAnalyzer.Contracts
{
    class RespEmployment
    {
        public double Value { get; set; }
        public static double Normalize(string rawData)
        {
            double numericValue = 0;
            string[] options =
            {
                "Not employed, and not looking for work",
                "Not employed, but looking for work",
                "Independent contractor, freelancer, or self-employed",
                "Employed part-time",
                "Employed full-time",
                "Retired"
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
