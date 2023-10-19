using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IVisit
    {
        public List<dynamic> GetVisits();

        public dynamic GetVisit(int? id);

        public void AddVisit(Visit visit);
    }
}
