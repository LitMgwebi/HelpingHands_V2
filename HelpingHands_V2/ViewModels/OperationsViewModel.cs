namespace HelpingHands_V2.ViewModels
{
    using HelpingHands_V2.Models;
    public class OperationsViewModel
    {
        public virtual IEnumerable<OperationHour>? OperationHours { get; set; } = new List<OperationHour>();

        public OperationHour? OperationHour { get; set; }
    }
}
