using System.ComponentModel.DataAnnotations;

namespace Blog_System.ViewModel
{
    public class ForgetPasswordViewModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
