using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IWound
    {
        public Task<IEnumerable<dynamic>> GetWounds();

        public Task<object> GetWound(int? id);

        public Task<dynamic> AddWound(Wound wound);
    }
}
