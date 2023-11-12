using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IReport
    {
        public Task<List<dynamic>> NurseAssignedConditions(int NurseId);
        public Task<List<dynamic>> NurseAssignedContracts(int id);
        public Task<List<dynamic>> NurseContractType(int id, string status);
        public Task<List<dynamic>> NurseContractVisits(int id);
        public Task<List<dynamic>> NurseVisitRange(int id, DateTime startDate, DateTime endDate);
        public Task<List<dynamic>> AvailableNurses(int suburbId);
        public Task<List<dynamic>> CareVisits(DateTime startDate, DateTime endDate);
        public Task<List<dynamic>> ContractStatus(string status);
        public Task<List<dynamic>> ContractVisits(int contractId);
        public Task<List<dynamic>> PatientContract(int PatientId);
    }
}
