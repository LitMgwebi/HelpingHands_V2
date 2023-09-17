using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IReport
    {
        public List<dynamic> NurseAssignedConditions(int NurseId);
        public List<dynamic> NurseAssignedContracts(int id);
        public List<dynamic> NurseContractType(int id, string status);
        public List<dynamic> NurseContractVisits(int id);
        public List<dynamic> NurseVisitRange(int id, DateTime startDate, DateTime endDate);
        public List<dynamic> AvailableNurses(int suburbId);
        public List<dynamic> CareVisits(DateTime startDate, DateTime endDate);
        public List<dynamic> ContractStatus(string status);
        public List<dynamic> ContractVisits(int contractId);
        public List<dynamic> PatientContract(int PatientId);
    }
}
