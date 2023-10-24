using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IOperation
    {
        public Task<IEnumerable<dynamic>> GetOperationHours();

        public Task<object> GetOperation(int? id);

        public Task<dynamic> AddOperationHours(OperationHour operation);
    }
}
