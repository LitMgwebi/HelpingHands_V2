using HelpingHands_V2.Models;

namespace HelpingHands_V2.ViewModels
{
    public class AdminViewModel
    {
        public List<CareContract>? AssignedContracts { get; set; }

        public List<CareContract>? ClosedContracts { get; set; }

        public List<Nurse>? WaitingNurses { get; set; }
    }
}
