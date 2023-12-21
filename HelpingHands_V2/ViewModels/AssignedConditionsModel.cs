using HelpingHands_V2.Models;

namespace HelpingHands_V2.ViewModels
{
    public class AssignedConditionsModel
    {
        public CareContract? Contract { get; set; }

        public EndUser? Patient { get; set; }

        public Suburb? Suburb { get; set; }

        public PatientCondition? PatientCondition { get; set; }

        public Condition? Condition { get; set; }

        public Wound? Wound { get; set; }
    }
}
