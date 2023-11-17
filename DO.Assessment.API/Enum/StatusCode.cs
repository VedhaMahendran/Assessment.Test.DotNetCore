using System.ComponentModel;

namespace DO.Assessment.API
{
    public enum EmailStatus
    {
        [Description("email containing OTP has been sent successfully.")]
        STATUS_EMAIL_OK,
        [Description("email address does not exist or sending to the email has failed.")]
        STATUS_EMAIL_FAIL,
        [Description("email address is invalid.")]
        STATUS_EMAIL_INVALID
    }
    public enum OtpStatus
    {
        [Description("OTP is valid and checked")]
        STATUS_OTP_OK,
        [Description("OTP is wrong after 10 tries")]
        STATUS_OTP_FAIL,
        [Description("timeout after 1 min")]
        STATUS_OTP_TIMEOUT
    }
}
