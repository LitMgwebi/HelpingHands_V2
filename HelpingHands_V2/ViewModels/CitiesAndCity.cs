namespace HelpingHands_V2.ViewModels
{
    using HelpingHands_V2.Models;
    public class CitiesAndCity
    {
        public virtual IEnumerable<City>? Cities {  get; set; } = new List<City>();

        public City? City { get; set; } = null;
    }
}
