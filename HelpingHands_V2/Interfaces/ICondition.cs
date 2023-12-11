using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface ICondition
    {
        public Task<IEnumerable<Condition>> GetConditions();

        public Task<Condition> GetCondition(int? id);

        public Task<dynamic> AddCondition(Condition condition);

        public Task<dynamic> UpdateCondition(Condition condition);

        public Task<dynamic> DeleteCondition(int id);
    }
}
