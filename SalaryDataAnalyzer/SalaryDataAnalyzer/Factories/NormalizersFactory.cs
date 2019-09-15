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

            yield return new GenericCountryNormalizer(new List<string>() { "Republic of Korea", "Bolivia", "Lebanon", "Panama", "Dominican Republic", "Zimbabwe", "Ethiopia", "Georgia", "Uzbekistan", "Venezuela", "Belarus", "Slovakia", "Malawi", "Uruguay", "Jordan", "South Korea", "Turkey" });
            yield return new GenericCountryNormalizer(new List<string>() { "Morocco", "Austria", "Thailand", "Croatia", "Argentina", "China", "Benin", "Sweden", "Nigeria", "Philippines", "United Arab Emirates", "Sri Lanka", "Luxembourg", "Bosnia and Herzegovina", "Ireland", "Poland", "Canada" });
            yield return new GenericCountryNormalizer(new List<string>() { "New Zealand", "Saudi Arabia", "Belgium", "Libyan Arab Jamahiriya", "Germany", "South Africa", "Japan", "Myanmar", "Uganda", "India", "Romania", "Ukraine", "United Kingdom", "Iceland", "Italy", "Brazil", "Australia" });
            yield return new GenericCountryNormalizer(new List<string>() { "United States", "Nepal", "Colombia", "France", "Bulgaria", "Algeria", "Switzerland", "Kuwait", "Norway", "Czech Republic", "Bangladesh", "Denmark", "Indonesia", "Nicaragua", "Honduras", "Ecuador", "Israel" });
            yield return new GenericCountryNormalizer(new List<string>() { "Hungary", "Spain", "Angola", "Mexico", "Serbia", "Portugal", "Egypt", "Gabon", "Cameroon", "Netherlands", "Iraq", "Greece", "Afghanistan", "Cyprus", "Mongolia", "Russian Federation", "Namibia" });
            yield return new GenericCountryNormalizer(new List<string>() { "Lithuania", "Taiwan", "Estonia", "Singapore", "Bahrain", "Kenya", "Iran", "Pakistan", "Trinidad and Tobago", "Finland", "Tajikistan", "Democratic Republic of the Congo", "Other Country (Not Listed Above)", "Slovenia", "Malaysia", "Albania", "Peru" });
            yield return new GenericCountryNormalizer(new List<string>() { "Viet Nam", "Malta", "Costa Rica", "Azerbaijan", "Chile", "Tunisia", "Hong Kong (S.A.R.)", "Madagascar", "Paraguay", "Latvia", "Ghana", "Republic of Moldova", "Cuba", "El Salvador", "Kyrgyzstan", "Guatemala", "Maldives" });
            yield return new GenericCountryNormalizer(new List<string>() { "Turkmenistan", "Mozambique", "Kazakhstan", "Mauritius", "The former Yugoslav Republic of Macedonia", "United Republic of Tanzania", "Armenia", "Syrian Arab Republic", "Somalia", "Andorra", "Montenegro", "Rwanda", "Cambodia", "Jamaica", "Sudan", "Oman", "Bhutan" });
        }
    }
}
