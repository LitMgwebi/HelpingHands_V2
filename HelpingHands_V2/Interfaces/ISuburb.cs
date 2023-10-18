using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface ISuburb
    {
        public List<dynamic> GetSuburbs();

        public dynamic GetSuburb(int? id);

        public void AddSuburb(Suburb suburb);
    }
}
