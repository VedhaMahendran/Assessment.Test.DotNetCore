using DO.Assessment.API.Configuration;
using DO.Assessment.API.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel;

namespace DO.Assessment.API.Services
{
    public class Generate_OTP_Module : IGenerate_OTP_Module
    {
        public EmailServer EmailServer { get; set; } = null!;
        private readonly IMemoryCache _cache;
        public Generate_OTP_Module(IMemoryCache cache)
        {
            _cache = cache;
        }
        public void Start(EmailServer _emailServer)
        {
            EmailServer = _emailServer;
        }
        public EmailStatus Generate_OTP_Email(string user_email, string otp)
        {
            try
            {
                if (!IsEmailAllowed(user_email))
                {
                    return EmailStatus.STATUS_EMAIL_INVALID;
                }

                Send_OTP_Module eMail = new Send_OTP_Module
                {
                    Subject = "Email verification OTP",
                    Body = string.Format("You OTP Code is {0}. The code is valid for 1 minute", otp),
                    ToAddress = user_email,
                    EmailServer = EmailServer
                };
                eMail.SendMail();
                return EmailStatus.STATUS_EMAIL_OK;
            }
            catch (Exception ex)
            {
                return EmailStatus.STATUS_EMAIL_FAIL;
            }
        }
        public void Save_OTP(string email, string otp)
        {
            // Store email and OTP in the cache with a 1-minute expiration
            _cache.Set(email, otp, TimeSpan.FromMinutes(1));
        }
        public OtpStatus Check_OTP(string email, string otp)
        {
            if (_cache.TryGetValue(email, out string? storedOtp))
            {
                // Check if the provided OTP matches the stored OTP
                if (otp.Equals(storedOtp, StringComparison.OrdinalIgnoreCase))
                    return OtpStatus.STATUS_OTP_OK;
            }

            int maxAttempts = 10;
            int attempts = 0;
            while (attempts < maxAttempts)
            {
                // Retrieve the stored OTP from the cache
                if (_cache.TryGetValue(email, out string? storedOtp1))
                {
                    // Check if the provided OTP matches the stored OTP
                    if (attempts == 10)
                        return OtpStatus.STATUS_OTP_FAIL;
                }
                attempts++;
            }

            return OtpStatus.STATUS_OTP_TIMEOUT;
        }
        public string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            if (field == null)
            {
                return null!;
            }

            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))!;

            return attribute == null ? value.ToString() : attribute.Description;
        }
        public string GenerateRandomOTP()
        {
            // Generate a 6-digit random OTP
            Random rand = new Random();
            return rand.Next(100000, 999999).ToString();
        }
        static bool IsEmailAllowed(string email)
        {
            // Check if the email address ends with the specified domain
            string allowedDomain = "gmail.com";
            bool isAllowed = email.EndsWith(allowedDomain, StringComparison.OrdinalIgnoreCase);
            return isAllowed;
        }
    }
}
