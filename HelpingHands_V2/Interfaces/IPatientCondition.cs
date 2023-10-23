using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IPatientCondition
    {
        public List<dynamic> GetPatientConditions();

        public List<dynamic> GetPatientConditionsByPatient(int? id);

        public dynamic GetPatientConditionByCondition(int? id);

        public dynamic GetOnePatientCondition(int? patientId, int? conditionId);

        public void AddPatientCondition(PatientCondition patientCondition);
    }
}
