using Blog_System.CustomVaildation;
using System.ComponentModel.DataAnnotations;

namespace Blog_System.ViewModel
{
    public class RegisterViewModel
    {
        [DataType(DataType.EmailAddress)]
        [UniqueEmail] // Custom
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [MaxLength(50)]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[A-Za-z]{2,20}$", ErrorMessage = "First name must be 2-20 letters with no spaces or numbers.")]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[A-Za-z]{2,20}$", ErrorMessage = "First name must be 2-20 letters with no spaces or numbers.")]
        public string LastName { get; set; }

        [MaxLength(250)]
        [Display(Name = "User Name")]
        [RegularExpression(@"^[a-zA-Z](?!.*[_.]{2})[a-zA-Z0-9._]{2,18}[a-zA-Z0-9]$",
        ErrorMessage = 
            "Username must start with a letter, be 4–20 characters, can include letters, numbers, _ and ., but not end with or have consecutive _ or .")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [AgeGreaterThanTenYears]
        public DateOnly BirthDate { get; set; }
    }
}
