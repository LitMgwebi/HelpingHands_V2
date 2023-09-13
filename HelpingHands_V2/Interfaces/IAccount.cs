using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IAccount
    {
        public Task<IEnumerable<EndUsers>> GetUser(string username);
    }
}
