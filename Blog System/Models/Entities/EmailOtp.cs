using System.ComponentModel.DataAnnotations;

namespace Blog_System.Models.Entities
{
    public class EmailOtp
    {
        public int Id { get; set; }

        public DateTime ExpiryTime { get; set; }

        public string OtpCode { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool IsUsed { get; set; }
    }
}
