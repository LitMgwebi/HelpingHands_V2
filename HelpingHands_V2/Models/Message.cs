namespace HelpingHands_V2.Models
{
    using System.Linq;
    using System.Collections.Generic;
    using MailKit.Net.Smtp;
    using MimeKit;

    public class Message
    {
        public List<MailboxAddress>? To { get; set; }
        
        public string? FullName { get; set; }

        public string? Username { get; set; }   

        public string? EmailType { get; set; }

        public Message(IEnumerable<string> to, string fullName, string username, string emailType)
        {
            To = new List<MailboxAddress>();

            To.AddRange(to.Select(x => new MailboxAddress(fullName, x)));
            FullName = fullName;
            Username = username;
            EmailType = emailType;
        }
    }
}
