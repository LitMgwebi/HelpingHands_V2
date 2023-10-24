using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IPrefferedSuburb
    {
        public Task<IEnumerable<dynamic>> GetPrefferedSuburbs();

        public Task<IEnumerable<dynamic>> GetPrefferedSuburbsByNurse(int? id);

        public Task<object> GetPrefferedSuburb(int? nurseId, int? suburbId);

        public Task<object> GetPrefferedSuburbBySuburb(int? id);

        public Task<dynamic> AddPrefferedSuburb(PrefferedSuburb prefferedSuburb);

    }
}
