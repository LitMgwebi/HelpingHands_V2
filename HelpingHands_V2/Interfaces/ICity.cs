using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface ICity
    {
        public Task<IEnumerable<dynamic>> GetCities();

        public Task<object> GetCity(int? id);

        public Task<dynamic> AddCity(City city);
    }
}
