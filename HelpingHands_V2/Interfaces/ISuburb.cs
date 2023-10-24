using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface ISuburb
    {
        public Task<IEnumerable<dynamic>> GetSuburbs();

        public Task<object> GetSuburb(int? id);

        public Task<dynamic> AddSuburb(Suburb suburb);
    }
}
