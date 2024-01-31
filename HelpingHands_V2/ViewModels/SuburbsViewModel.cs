namespace HelpingHands_V2.ViewModels
{
    using HelpingHands_V2.Models;
    public class SuburbsViewModel
    {
        public virtual IEnumerable<Suburb>? Suburbs { get; set; } = new List<Suburb>();

        public Suburb? Suburb { get; set; } = null;
    }
}
