using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HelpingHands_V2.Models;

public partial class PatientCondition
{
    [Display(Name = "Patient")]
    public int? PatientId { get; set; }

    [Display(Name = "Condition")]
    [Required(ErrorMessage = "Please select a condition")]
    public int? ConditionId { get; set; }

    public bool Active { get; set; } = true;

    public virtual Condition? Condition { get; set; }

    public virtual EndUser? Patient { get; set; }
}
