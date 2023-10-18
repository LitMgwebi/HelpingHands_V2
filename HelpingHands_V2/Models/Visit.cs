using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HelpingHands_V2.Models;

public partial class Visit
{
    public int VisitId { get; set; }

    [DisplayName("Contract Number")]
    public int ContractId { get; set; }

    [DisplayName("Visit Date")]
    public DateTime VisitDate { get; set; }

    [DisplayName("Approximate Time")]
    public TimeSpan? ApproxTime { get; set; }

    [DisplayName("Arrival Time")]
    public TimeSpan? Arrival { get; set; }

    [DisplayName("Departure Time")]
    public TimeSpan? Departure { get; set; }

    [DisplayName("Wound Condition")]
    public string? WoundCondition { get; set; }

    public string? Note { get; set; }

    public bool Active { get; set; } = true;

    public virtual CareContract Contract { get; set; } = null!;
}
