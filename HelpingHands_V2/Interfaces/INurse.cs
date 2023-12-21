using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface INurse
    {
        public Task<IEnumerable<Nurse>> GetNurses();
        public Task<IEnumerable<EndUser>> GetUsersByIDs(IEnumerable<Nurse> Nurses);

        public Task<Nurse> GetNurse(int? id);

        public Task<dynamic> AddNurse(Nurse nurse);

        public Task<dynamic> UpdateNurse(Nurse nurse);

        public Task<dynamic> DeleteNurse(int id);
    }
}
