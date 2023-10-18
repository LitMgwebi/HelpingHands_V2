using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface ICondition
    {
        public List<dynamic> GetConditions();

        public dynamic GetCondition(int? id);

        public void AddCondition(Condition condition);
    }
}
