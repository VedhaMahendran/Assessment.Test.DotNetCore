namespace DO.Assessment.API.Configuration
{
    public class EmailServer
    {
        public string SMTPServer { get; set; } = null!;
        public int SMTPPort { get; set; }
        public string SMTPUser { get; set; } = null!;
        public string SMTPUserPassword { get; set; } = null!;
        public string FromMailAddressDisplayName { get; set; } = null!;
        public string EmailFromAdd { get; set; } = null!;

    }
}
