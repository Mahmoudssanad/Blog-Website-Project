using Blog_System.Models.Entities;

namespace Blog_System.Servicies
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);

        Task Add(EmailOtp emailOtp);
        Task UpdateAsync(EmailOtp emailOtp);

        Task<EmailOtp> FindByEmailAsync(string email);
    }
}
