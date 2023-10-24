namespace HelpingHands_V2.Interfaces
{
    public interface IPatient
    {
        public Task<IEnumerable<dynamic>> GetPatients();

        public Task<object> GetPatient(int? id);
    }
}
