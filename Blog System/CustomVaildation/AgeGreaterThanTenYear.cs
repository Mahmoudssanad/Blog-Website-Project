using System.ComponentModel.DataAnnotations;

namespace Blog_System.CustomVaildation
{
    public class AgeGreaterThanTenYears : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is not DateOnly birthdate)
            {
                return new ValidationResult("Invaild Date syntax");
            }

            var today = DateTime.Today;

            var age = today.Year - birthdate.Year;

            if(birthdate.Year > today.Year)
            {
                return new ValidationResult("Can not be in the future");
            }

            if(age < 10)
            {
                return new ValidationResult("You must be at least 10 years old.");
            }

            return ValidationResult.Success;
        }
    }
}
