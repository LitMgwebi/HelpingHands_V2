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
            var emailType = message.EmailType;
            MimeMessage emailMessage = new MimeMessage();

            if(emailType == "patient_registering")
                emailMessage = PatientRegisteringMessage(message);
            else if(emailType == "nurse_registering")
                emailMessage = NurseRegisteringMessage(message);
            else if(emailType == "patient_contract")
                emailMessage = PatientContract(message);
            else if(emailType == "nurse_contract")
                emailMessage = NurseContract(message);
            else if(emailType == "nurse_approval")
                emailMessage = NurseApproval(message);

            Send(emailMessage);
        }

        private void Send(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_email.SmtpServer, _email.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_email.Username, _email.Password);
                    client.Send(message);
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        private MimeMessage PatientRegisteringMessage(Message message)
        {
            string content = $"Hello {message.FullName}.\n\nWe at Helping Hands are extremely happy to have you join our family.\n\nThe user you have been allocated is: {message.Username}.\nPlease user this email and the password you created to log on to the system.\n\nKind Regards,\nHelping Hands Team.\n\n*This is an automated response*";
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Helping Hands Automated Service", _email.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = "Welcome to Helping Hands";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = content };

            return emailMessage;
        }

        private MimeMessage NurseRegisteringMessage(Message message)
        {
            string content = $"Hello {message.FullName}.\n\nWe at Helping Hands are extremely grateful for your interest in being one of our nurses.\n\n Your application is currently under review.\nYou will receive an email in regards to your application's progress.\n\nKind Regards,\nHelping Hands Team.\n\n*This is an automated response*";
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Helping Hands Automated Service", _email.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = "Await Helping Hands Approval";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = content };

            return emailMessage;
        }

        private MimeMessage NurseApproval(Message message)
        {
            string content = $"Hello {message.FullName}.\n\nWe at Helping Hands are extremely happy to have you join our family.\n\nThe user you have been allocated is: {message.Username}.\nPlease user this email and the password you created to log on to the system.\n\nKind Regards,\nHelping Hands Team.\n\n*This is an automated response*";
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Helping Hands Automated Service", _email.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = "Welcome to Helping Hands";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = content };

            return emailMessage;
        }
        
        private MimeMessage PatientContract(Message message)
        {
            string content = $"Hello {message.FullName}.\n\nYou have recently created a Care Contract.\n\nLog on to your Helping Hands account to view the contract details.\n\nKind Regards,\nHelping Hands Team.\n\n*This is an automated response*";
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Helping Hands Automated Service", _email.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = "You have created a Contract";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = content };

            return emailMessage;
        }
        private MimeMessage NurseContract(Message message)
        {
            string content = $"Hello {message.FullName}.\n\nYou have recently chosen to undertake a new contract.\n\nLog on to your Helping Hands account to view the contract details.\n\nKind Regards,\nHelping Hands Team.\n\n*This is an automated response*";
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Helping Hands Automated Service", _email.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = "You have undertaken a contract";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = content };

            return emailMessage;
        }

    }
}
