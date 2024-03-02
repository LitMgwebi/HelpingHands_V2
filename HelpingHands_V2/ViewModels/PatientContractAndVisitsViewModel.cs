using HelpingHands_V2.Models;

namespace HelpingHands_V2.ViewModels
{
    public class PatientContractAndVisitsViewModel
    {
        public virtual IEnumerable<CareContract> PatientContracts { get; set; } = new List<CareContract>();

        public virtual IEnumerable<Visit> LatestVisits { get; set; } = new List<Visit>();
    }
}
