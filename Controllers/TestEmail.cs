using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TenderAPI.Services.EmailServices;

namespace TenderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestEmail : Controller
    {
        private readonly IEmailService _emailService;
        public TestEmail(IEmailService emailService)
        {
            _emailService = emailService;
        }
        // GET: TestEmail
        [HttpGet]
        public ActionResult Send()
        {
            try
            {
                _emailService.SendEmail(new Models.EmailDto());

                return Ok();
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
