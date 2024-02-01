using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using TenderAPI.Models;
using MailKit.Net.Smtp;

namespace TenderAPI.Services.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        public async void SendEmail(EmailDto request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(_configuration.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);

            var emailUsername = _configuration.GetSection("EmailUsername").Value;
            var emailPassword = _configuration.GetSection("EmailPassword").Value;

            smtp.Authenticate(emailUsername, emailPassword);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}  
