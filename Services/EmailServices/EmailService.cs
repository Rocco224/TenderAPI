using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using TenderAPI.Models;
using MailKit.Net.Smtp;
using TenderAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace TenderAPI.Services.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly TenderDbContext _context;

        public EmailService(IConfiguration configuration, TenderDbContext context) 
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task SendEmail(EmailDto request)
        {
            var expiringPractices = await _context.ExpiringPractices.ToListAsync();

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            // pratiche in scadenza
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };
            // la pratica {{objecy}} del cliente {{customer}} e' in scadenza giorno {{date expire}}

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_configuration.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);

            var emailUsername = _configuration.GetSection("EmailUsername").Value;
            var emailPassword = _configuration.GetSection("EmailPassword").Value;

            await smtp.AuthenticateAsync(emailUsername, emailPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}  
