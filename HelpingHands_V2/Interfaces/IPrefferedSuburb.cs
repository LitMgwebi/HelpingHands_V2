using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IPrefferedSuburb
    {
        public List<dynamic> GetPrefferedSuburbs();

        public List<dynamic> GetPrefferedSuburbsByNurse(int? id);

        public dynamic GetPrefferedSuburb(int? nurseId, int? suburbId);

        public dynamic GetPrefferedSuburbBySuburb(int? id);

        public void AddPrefferedSuburb(PrefferedSuburb prefferedSuburb);

    }
}
