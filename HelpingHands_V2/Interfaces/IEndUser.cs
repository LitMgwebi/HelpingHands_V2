using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IEndUser
    {
        public Task<IEnumerable<EndUser>> GetManagers();
        public Task<IEnumerable<EndUser>> GetUsers();

        public Task<EndUser> GetUserById(int? id);
        public Task<EndUser> GetUserByUsername(string username);


        public Task<dynamic> AddUser(EndUser user);
        public Task<dynamic> UpdateUser(dynamic user);
        public Task<dynamic> DeleteUser(int id);
    }
}
