using DO.Assessment.API.Configuration;
using System.Net.Mail;
using System.Text;

namespace DO.Assessment.API.Services
{
    public class Send_OTP_Module
    {
        public EmailServer EmailServer { get; set; } = null!;
        public string ToAddress { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!; 
        public void SendMail()
        {
            if ((string.IsNullOrEmpty(Subject)) ||
           (string.IsNullOrEmpty(Body)) ||
           (string.IsNullOrEmpty(EmailServer.EmailFromAdd)) ||
           (string.IsNullOrEmpty(ToAddress)))
            {
                throw new ArgumentNullException();
            }

            MailMessage mail = new MailMessage();
            MailAddress origMA = new MailAddress(EmailServer.EmailFromAdd, EmailServer.FromMailAddressDisplayName);

            foreach (string mID in ToAddress.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                mail.To.Add(new MailAddress(mID));
            }

            mail.From = origMA;
            mail.Subject = Subject;
            mail.IsBodyHtml = true;
            string _body = Body;
            mail.Body = _body;
            mail.BodyEncoding = Encoding.UTF8;
            SmtpClient mailClient = new SmtpClient(EmailServer.SMTPServer, EmailServer.SMTPPort);
            mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            mailClient.UseDefaultCredentials = false;
            mailClient.EnableSsl = true;
            mailClient.Credentials = new System.Net.NetworkCredential(EmailServer.SMTPUser, EmailServer.SMTPUserPassword);
            mailClient.Send(mail);
        }
    }
}
