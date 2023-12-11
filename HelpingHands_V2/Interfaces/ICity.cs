using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface ICity
    {
        public Task<IEnumerable<City>> GetCities();

        public Task<City> GetCity(int? id);

        public Task<dynamic> AddCity(City city);

        public Task<dynamic> UpdateCity(City city);

        public Task<dynamic> DeleteCity(int id);
    }
}
