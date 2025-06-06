using System.ComponentModel.DataAnnotations;

namespace Blog_System.ViewModel
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [StringLength(100, ErrorMessage = "The password must be at least {2} characters.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Confirm new password dos not match")]
        public string ConfirmPassword { get; set; }
    }
}
