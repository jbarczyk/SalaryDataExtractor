
namespace SalaryDataAnalyzer.Contracts
{
    class RespDevType
    {
        public double Value { get; set; }
        public static double Normalize(string rawData)
        {
            double numericValue = 0;
            string[] options =
            {
                "Back-end developer",
                "C-suite executive (CEO, CTO, etc.)",
                "Data or business analyst",
                "Data scientist or machine learning specialist",
                "Database administrator",
                "Designer",
                "Desktop or enterprise applications developer",
                "DevOps specialist",
                "Educator or academic researcher",
                "Embedded applications or devices developer",
                "Engineering manager",
                "Front-end developer",
                "Full-stack developer",
                "Game or graphics developer",
                "Marketing or sales professional",
                "Mobile developer",
                "Product manager",
                "QA or test developer",
                "Student",
                "System administrator"
            };

            //DevType has many values sorted from the most important
            var separated = rawData.Split(';');

            int index = 1;
            foreach (string option in options)
            {
                if (option == separated[0])
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
