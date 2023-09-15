namespace HelpingHands_V2.Interfaces
{
    public interface IManager
    {
        public List<dynamic> AvailableNurses(int suburbId);
        public List<dynamic> CareVisits(DateTime startDate, DateTime endDate);
        public List<dynamic> ContractStatus(string status);
        public List<dynamic> ContractVisits(int contractId);
        public List<dynamic> PatientContract(int PatientId);

    }
}
