using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface INurse
    {
        public Task<IEnumerable<dynamic>> GetNurses();

        public Task<object> GetNurse(int? id);

        public Task<dynamic> AddNurse(Nurse nurse);

        public Task<dynamic> UpdateNurse(Nurse nurse);

        public Task<dynamic> DeleteNurse(int id);
    }
}
