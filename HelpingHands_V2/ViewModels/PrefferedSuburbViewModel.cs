namespace HelpingHands_V2.ViewModels
{
    using HelpingHands_V2.Models;
    public class PrefferedSuburbViewModel
    {
        public int? NurseId { get; set; }

        public IEnumerable<PrefferedSuburb> Suburbs { get; set; } = new List<PrefferedSuburb>();

        public virtual Suburb? Suburb { get; set; }
    }
}
