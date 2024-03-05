using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface INurse
    {
        public Task<List<Nurse>> GetNurses();
        public Task<List<Nurse>> GetNursesWaiting();
        public Task<List<EndUser>> GetUsersByIDs(List<Nurse> Nurses);

        public Task<Nurse> GetNurse(int? id);

        public Task<dynamic> AddNurse(Nurse nurse);

        public Task<dynamic> UpdateNurse(Nurse nurse);

        public Task<dynamic> DeleteNurse(int id);
    }
}
