namespace HelpingHands_V2.Interfaces
{
    public interface IWound
    {
        public List<dynamic> GetWounds();

        public dynamic GetWound(int? id);
    }
}
