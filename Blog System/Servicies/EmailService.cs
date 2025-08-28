
using Blog_System.Models.Data;
using Blog_System.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace Blog_System.Servicies
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public EmailService(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var host = _configuration["Mailsettings:Host"];
            var port = int.Parse(_configuration["Mailsettings:Port"]);
            var userName = _configuration["Mailsettings:UserName"];
            var email = _configuration["Mailsettings:Email"];
            var password = _configuration["Mailsettings:Password"];

            using var smtp = new SmtpClient(host)
            {
                Port = port,
                Credentials = new NetworkCredential(email, password),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                Subject = subject,
                Body = body,
                From = new MailAddress(email)
            };

            mail.To.Add(toEmail);

            await smtp.SendMailAsync(mail);
        }

        public async Task Add(EmailOtp emailOtp)
        {
            var existing = await _context.EmailOtps
                .Where(e => e.Email == emailOtp.Email)
                .ToListAsync();

            if (existing.Any())
            {
                var otp = existing.First();

                otp.OtpCode = emailOtp.OtpCode;
                otp.IsUsed = false;
                otp.ExpiryTime = emailOtp.ExpiryTime;

                _context.EmailOtps.Update(otp);
                await _context.SaveChangesAsync();
            }

            if (emailOtp != null)
            {
                await _context.EmailOtps.AddAsync(emailOtp);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<EmailOtp> FindByEmailAsync(string email)
        {
            var found = await _context.EmailOtps.FirstOrDefaultAsync(x => x.Email == email);
            if (found == null)
            { }
            return found;
        }

        public async Task UpdateAsync(EmailOtp emailOtp)
        {
            var found = await _context.EmailOtps.FirstOrDefaultAsync(x => x.Email == emailOtp.Email);

            if(found != null)
            {
                found.OtpCode = emailOtp.OtpCode;
                found.ExpiryTime = emailOtp.ExpiryTime;
                found.IsUsed = emailOtp.IsUsed;
                found.Email = emailOtp.Email;

                await _context.SaveChangesAsync();
            }

        }
    }
}
