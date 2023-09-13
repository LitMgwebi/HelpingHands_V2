using System;
using System.Collections.Generic;

namespace HelpingHands_V2.Models;

public partial class Visit
{
    public int VisitId { get; set; }

    public int ContractId { get; set; }

    public DateTime VisitDate { get; set; }

    public TimeSpan? ApproxTime { get; set; }

    public TimeSpan? Arrival { get; set; }

    public TimeSpan? Departure { get; set; }

    public string? WoundCondition { get; set; }

    public string? Note { get; set; }

    public bool Active { get; set; }

    public virtual CareContract Contract { get; set; } = null!;
}
