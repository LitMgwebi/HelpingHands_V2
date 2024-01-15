using HelpingHands_V2.Models;
using HelpingHands_V2.ViewModels;

namespace HelpingHands_V2.Interfaces
{
    public interface IReport
    {
        public Task<List<AssignedConditionsModel>> NurseAssignedConditions(int? NurseId);
        public Task<List<CareContract>> NurseAssignedContracts(int? id);
        public Task<List<CareContract>> NurseContractType(int? id, string? status);
        public Task<List<VisitRange>> NurseVisitRange(int? id, DateTime? startDate, DateTime? endDate);
        public Task<List<CareContract>> NurseContractsByGrades(int? NurseId);
        public Task<List<dynamic>> AvailableNurses(int? suburbId);
        public Task<List<CareContract>> CareVisits(int? id, DateTime? startDate, DateTime? endDate);
        public Task<List<CareContract>> ContractStatus(string? status);
        public Task<List<Visit>> ContractVisits(int? contractId);
        public Task<List<CareContract>> PatientContract(int? PatientId);
    }
}
