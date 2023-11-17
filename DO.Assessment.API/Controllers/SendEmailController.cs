using Microsoft.AspNetCore.Mvc;
using DO.Assessment.API.Configuration;
using DO.Assessment.API.Interfaces; 

namespace DO.Assessment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendEmailController : ControllerBase
    {
        private readonly IGenerate_OTP_Module _generate_OTP_Module;
        public IConfiguration Configuration { get; }
        public SendEmailController(IConfiguration configuration, IGenerate_OTP_Module generate_OTP_Module)
        {
            Configuration = configuration;
            _generate_OTP_Module = generate_OTP_Module;
        }

        [HttpGet(Name = "Generate_OTP_Email")]
        public IActionResult Generate_OTP_Email(string email_address)
        {
            EmailServer emailServer = new EmailServer();
            Configuration.GetSection("EmailServer").Bind(emailServer);
            string generatedOtp = _generate_OTP_Module.GenerateRandomOTP();
            _generate_OTP_Module.Start(emailServer);
            var statusCode = _generate_OTP_Module.Generate_OTP_Email(email_address, generatedOtp); 
            // Save OTP in the cache for 1 minute
            if (statusCode == EmailStatus.STATUS_EMAIL_OK)
            {
                _generate_OTP_Module.Save_OTP(email_address, generatedOtp); 
            }
            return Ok(_generate_OTP_Module.GetEnumDescription(statusCode));
        }

        [HttpPost("Check_OTP")]
        public IActionResult Check_OTP(string email, string otp)
        {
            // Check the OTP
            var statusCode = _generate_OTP_Module.Check_OTP(email, otp);
            return Ok(_generate_OTP_Module.GetEnumDescription(statusCode));
        }
    }
}
