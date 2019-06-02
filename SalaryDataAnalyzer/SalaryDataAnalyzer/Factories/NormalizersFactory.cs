using System.Collections.Generic;
using SalaryDataAnalyzer.Contracts;

namespace SalaryDataAnalyzer.Factories
{
    public class NormalizersFactory
    {
        public IEnumerable<ResponseNormalizerBase> CreateNormalizers()
        {
            yield return new ConvertedSalaryNormalizer();
            yield return new CompanySizeNormalizer();
            yield return new EmploymentNormalizer();
            yield return new FormalEducationNormalizer();
            yield return new StudentNormalizer();
            yield return new YearsCodingNormalizer();

            yield return new GenericDevTypeNormalizer("Back-end developer");
            yield return new GenericDevTypeNormalizer("C-suite executive (CEO, CTO, etc.)");
            yield return new GenericDevTypeNormalizer("Data or business analyst");
            yield return new GenericDevTypeNormalizer("Data scientist or machine learning specialist");
            yield return new GenericDevTypeNormalizer("Database administrator");
            yield return new GenericDevTypeNormalizer("Designer");
            yield return new GenericDevTypeNormalizer("Desktop or enterprise applications developer");
            yield return new GenericDevTypeNormalizer("DevOps specialist");
            yield return new GenericDevTypeNormalizer("Educator or academic researcher");
            yield return new GenericDevTypeNormalizer("Embedded applications or devices developer");
            yield return new GenericDevTypeNormalizer("Engineering manager");
            yield return new GenericDevTypeNormalizer("Front-end developer");
            yield return new GenericDevTypeNormalizer("Full-stack developer");
            yield return new GenericDevTypeNormalizer("Game or graphics developer");
            yield return new GenericDevTypeNormalizer("Marketing or sales professional");
            yield return new GenericDevTypeNormalizer("Mobile developer");
            yield return new GenericDevTypeNormalizer("Product manager");
            yield return new GenericDevTypeNormalizer("QA or test developer");
            yield return new GenericDevTypeNormalizer("Student");
            yield return new GenericDevTypeNormalizer("System administrator");
        }
    }
}
