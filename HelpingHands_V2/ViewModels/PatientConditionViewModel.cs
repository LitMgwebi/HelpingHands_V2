using HelpingHands_V2.Models;

namespace HelpingHands_V2.ViewModels
{
    public class PatientConditionViewModel
    {
        public int? PatientId { get; set; }

        public IEnumerable<PatientCondition>? Conditions { get; set; } = new List<PatientCondition>();

        public virtual Condition? Condition { get; set; }
    }
}
