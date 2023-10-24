using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IContract
    {
        public Task<IEnumerable<dynamic>> GetContracts();

        public Task<object> GetContract(int? id);

        public Task<dynamic> AddContract(CareContract contract);
    }
}
