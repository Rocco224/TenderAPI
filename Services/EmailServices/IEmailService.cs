using TenderAPI.Models;

namespace TenderAPI.Services.EmailServices
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request);
    }
}
