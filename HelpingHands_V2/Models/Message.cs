namespace HelpingHands_V2.Models
{
    using System.Linq;
    using System.Collections.Generic;
    using MailKit.Net.Smtp;
    using MimeKit;

    public class Message
    {
        public List<MailboxAddress>? To { get; set; }
        
        public string? Subject { get; set; }

        public string? Content { get; set; }

        public Message(IEnumerable<string> to, string subject, string content, string receiverName)
        {
            To = new List<MailboxAddress>();

            To.AddRange(to.Select(x => new MailboxAddress(receiverName, x)));
            Subject = subject;
            Content = content;
        }
    }
}
