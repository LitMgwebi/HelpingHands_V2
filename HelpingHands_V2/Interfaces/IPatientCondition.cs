using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IPatientCondition
    {
        public Task<IEnumerable<PatientCondition>> GetPatientConditions();

        public Task<IEnumerable<PatientCondition>> GetPatientConditionsByPatient(int? id);

        public Task<PatientCondition> GetOnePatientCondition(int? patientId, int? conditionId);

        public Task<dynamic> AddPatientCondition(PatientCondition patientCondition);

        public Task<dynamic> UpdatePatientCondition(PatientCondition patientCondition);

        public Task<dynamic> DeletePatientCondition(PatientCondition patientCondition);
    }
}
