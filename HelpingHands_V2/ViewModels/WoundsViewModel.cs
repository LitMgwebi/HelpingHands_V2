namespace HelpingHands_V2.ViewModels
{
    using HelpingHands_V2.Models;
    public class WoundsViewModel
    {
        public virtual IEnumerable<Wound> Wounds { get; set; } = new List<Wound>();

        public Wound? Wound { get; set; } = null;
    }
}
