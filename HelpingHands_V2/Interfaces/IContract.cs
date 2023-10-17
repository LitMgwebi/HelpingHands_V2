namespace HelpingHands_V2.Interfaces
{
    public interface IContract
    {
        public List<dynamic> GetContracts();

        public dynamic GetContract(int? id);
    }
}
