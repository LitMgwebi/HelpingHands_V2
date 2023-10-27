using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IVisit
    {
        public Task<IEnumerable<dynamic>> GetVisits();

        public Task<object> GetVisit(int? id);

        public Task<dynamic> AddVisit(Visit visit);

        public Task<dynamic> UpdateVisit(Visit visit);

        public Task<dynamic> DeleteVisit(int id);
    }
}
