using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IEndUser
    {
        public EndUser GetUser(string username);
        public List<dynamic> GetManagers();
    }
}
