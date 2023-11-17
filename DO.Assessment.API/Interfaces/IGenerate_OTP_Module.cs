using DO.Assessment.API.Configuration;

namespace DO.Assessment.API.Interfaces
{
    public interface IGenerate_OTP_Module
    {
        void Start(EmailServer _emailServer);
        EmailStatus Generate_OTP_Email(string user_email, string otp);
        void Save_OTP(string email, string otp);
        OtpStatus Check_OTP(string email, string otp);
        string GetEnumDescription(Enum value);
        public string GenerateRandomOTP();
    }
}
