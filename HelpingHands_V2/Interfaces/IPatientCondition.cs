using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IPatientCondition
    {
        public Task<IEnumerable<dynamic>> GetPatientConditions();

        public Task<IEnumerable<dynamic>> GetPatientConditionsByPatient(int? id);

        public Task<object> GetPatientConditionByCondition(int? id);

        public Task<object> GetOnePatientCondition(int? patientId, int? conditionId);

        public Task<dynamic> AddPatientCondition(PatientCondition patientCondition);
    }
}
