using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface ISuburb
    {
        public Task<IEnumerable<Suburb>> GetSuburbs();

        public Task<Suburb> GetSuburb(int? id);

        public Task<dynamic> AddSuburb(Suburb suburb);

        public Task<dynamic> UpdateSuburb(Suburb suburb);

        public Task<dynamic> DeleteSuburb(int id);
    }
}
