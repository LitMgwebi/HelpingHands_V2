using HelpingHands_V2.Models;

namespace HelpingHands_V2.ViewModels
{
    public class ConditionsViewModel
    {
        public virtual IEnumerable<Condition>? Conditions { get; set; } = new List<Condition>();

        public Condition? Condition { get; set; } = null;
    }
}
