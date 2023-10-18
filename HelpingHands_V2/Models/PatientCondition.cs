using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HelpingHands_V2.Models;

public partial class PatientCondition
{
    [Display(Name = "Patient")]
    public int PatientId { get; set; }

    [Display(Name = "Condition")]
    public int ConditionId { get; set; }

    public bool Active { get; set; } = true;

    public virtual Condition Condition { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}
