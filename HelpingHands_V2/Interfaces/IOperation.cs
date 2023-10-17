namespace HelpingHands_V2.Interfaces
{
    public interface IOperation
    {
        public List<dynamic> GetOperationHours();

        public dynamic GetOperation(int? id);
    }
}
