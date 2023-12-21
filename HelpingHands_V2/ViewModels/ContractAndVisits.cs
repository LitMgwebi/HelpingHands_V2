using HelpingHands_V2.Models;

namespace HelpingHands_V2.ViewModels
{
    public class ContractAndVisits
    {
        public CareContract? Contract { get; set; }

        public IEnumerable<Visit>? Visits { get; set; }

        public Visit? Visit { get; set; }
    }
}
