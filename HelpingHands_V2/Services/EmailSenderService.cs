using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace HelpingHands_V2.Services
{
    public class EmailSenderService: IEmailSender
    {
        private readonly Emailer _email;

        public EmailSenderService(Emailer email)
        {
            _email = email;
        }

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Helping Hands Automated Service", _email.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }

        private void Send(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.ConnectAsync(_email.SmtpServer, _email.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.AuthenticateAsync(_email.Username, _email.Password);
                    client.SendAsync(message);
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    throw;
                }
                finally
                {
                    client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
