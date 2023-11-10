using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IEndUser
    {
        public Task<IEnumerable<dynamic>> GetManagers();
        public Task<IEnumerable<dynamic>> GetUsers();

        public Task<dynamic> GetUserById(int? id);
        public Task<dynamic> GetUserByUsername(string username);


        public Task<dynamic> AddUser(EndUser user);
        public Task<dynamic> UpdateUser(dynamic user);
        public Task<dynamic> DeleteUser(int id);
    }
}
