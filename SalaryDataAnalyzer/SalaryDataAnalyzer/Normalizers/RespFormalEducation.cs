
namespace SalaryDataAnalyzer.Contracts
{
    class RespFormalEducation
    {
        public double Value { get; set; }
        public static double Normalize(string rawData)
        {
            double numericValue = 0;
            string[] options =
            {
                "Associate degree",
                "Bachelor’s degree (BA, BS, B.Eng., etc.)",
                "I never completed any formal education",
                "Master’s degree (MA, MS, M.Eng., MBA, etc.)",
                "Other doctoral degree (Ph.D, Ed.D., etc.)",
                "Primary/elementary school",
                "Professional degree (JD, MD, etc.)",
                "Secondary school (e.g. American high school, German Realschule or Gymnasium, etc.)",
                "Some college/university study without earning a degree"
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
