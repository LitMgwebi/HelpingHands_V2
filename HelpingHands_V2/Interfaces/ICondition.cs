namespace HelpingHands_V2.Interfaces
{
    public interface ICondition
    {
        public List<dynamic> GetConditions();

        public dynamic GetCondition(int? id);
    }
}
