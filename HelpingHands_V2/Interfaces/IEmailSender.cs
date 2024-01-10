namespace HelpingHands_V2.Interfaces
{
    using HelpingHands_V2.Models; 
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
