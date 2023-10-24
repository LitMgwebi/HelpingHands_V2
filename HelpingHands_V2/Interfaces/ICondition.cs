using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface ICondition
    {
        public Task<IEnumerable<dynamic>> GetConditions();

        public Task<object> GetCondition(int? id);

        public Task<dynamic> AddCondition(Condition condition);
    }
}
