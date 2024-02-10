using TenderAPI.Models;

namespace TenderAPI.Services.EmailServices
{
    public interface IEmailService
    {
        Task SendEmail(EmailDto request);
    }
}
