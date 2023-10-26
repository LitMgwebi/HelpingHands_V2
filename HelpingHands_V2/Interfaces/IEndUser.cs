using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IEndUser
    {
        public Task<IEnumerable<dynamic>> GetManagers();
        public Task<IEnumerable<dynamic>> GetUsers();

        public Task<object> GetUserById(int? id);
        public Task<EndUser> GetUserByUsername(string username);


        public Task<dynamic> AddUser(EndUser user);
        public Task<dynamic> UpdateUser(EndUser user);
        public Task<dynamic> DeleteUser(int id);
    }
}
