using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IOperation
    {
        public List<dynamic> GetOperationHours();

        public dynamic GetOperation(int? id);

        public void AddOperationHours(OperationHour operation);
    }
}
