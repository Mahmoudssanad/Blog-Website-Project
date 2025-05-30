using Blog_System.Models.Data;
using System.ComponentModel.DataAnnotations;

namespace Blog_System.CustomVaildation
{
    public class UniqueEmail : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var email = value as string;

            // Dbcontext عشان أجيب ال Dependancy Injection عملت 
            var dbcontext = validationContext.GetService<AppDbContext>();

            var found = dbcontext.Users.Any(x => x.Email == email);

            if (found)
            {
                return new ValidationResult("This email already registered.");
            }

            return ValidationResult.Success;
        }
    }
}
