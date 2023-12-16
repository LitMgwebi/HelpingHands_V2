using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IContract
    {
        public Task<IEnumerable<CareContract>> GetContracts();

        public Task<CareContract> GetContract(int? id);

        public Task<dynamic> AddContract(CareContract contract);

        public Task<dynamic> UpdateContract(CareContract contract);

        public Task<dynamic> DeleteContract(int id);
    }
}
