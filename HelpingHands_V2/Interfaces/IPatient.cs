using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IPatient
    {
        public Task<IEnumerable<dynamic>> GetPatients();

        public Task<object> GetPatient(int? id);

        public Task<dynamic> AddPateint(Patient patient);

        public Task<dynamic> UpdatePatient(Patient patient);

        public Task<dynamic> DeletePatient(int id);
    }
}
