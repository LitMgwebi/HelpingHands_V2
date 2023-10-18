using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IWound
    {
        public List<dynamic> GetWounds();

        public dynamic GetWound(int? id);

        public void AddWound(Wound wound);
    }
}
