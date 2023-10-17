using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IEndUser
    {
        public EndUser GetUserByUsername(string username);
        public List<dynamic> GetManagers();

        public dynamic GetUserById(int? id);
    }
}
