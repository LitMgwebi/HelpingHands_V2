namespace HelpingHands_V2.Models
{
    public class Emailer
    {
        public string? From { get; set; }

        public string? SmtpServer { get; set; }

        public int Port { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }
    }
}
