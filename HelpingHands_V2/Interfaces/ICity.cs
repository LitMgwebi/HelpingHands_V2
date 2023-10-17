namespace HelpingHands_V2.Interfaces
{
    public interface ICity
    {
        public List<dynamic> GetCities();

        public dynamic GetCity(int? id);
    }
}
