using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface INurse
    {
        public List<dynamic> NurseAssignedConditions(int NurseId);
        public List<dynamic> NurseAssignedContracts(int id);
        public List<dynamic> NurseContractType(int id, string status);
        public List<dynamic> NurseContractVisits(int id);
        public List<dynamic> NurseVisitRange(int id, DateTime startDate, DateTime endDate);
        public List<dynamic> GetNurses();
    }
}
