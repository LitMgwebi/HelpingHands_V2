using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IContract
    {
        public List<dynamic> GetContracts();

        public dynamic GetContract(int? id);

        public void AddContract(CareContract contract);
    }
}
