namespace HelpingHands_V2.Interfaces
{
    public interface IPatientCondition
    {
        public List<dynamic> GetPatientConditions();

        public dynamic GetPatientConditionByPatient(int? id);

        public dynamic GetPatientConditionByCondition(int? id);
    }
}
