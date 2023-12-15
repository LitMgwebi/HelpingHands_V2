using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IPrefferedSuburb
    {
        public Task<IEnumerable<PrefferedSuburb>> GetPrefferedSuburbs();

        public Task<IEnumerable<PrefferedSuburb>> GetPrefferedSuburbsByNurse(int? id);

        public Task<PrefferedSuburb> GetPrefferedSuburb(int? nurseId, int? suburbId);

        public Task<dynamic> AddPrefferedSuburb(PrefferedSuburb prefferedSuburb);

        public Task<dynamic> DeletePrefferedSuburb(PrefferedSuburb prefferedSuburb);

    }
}
