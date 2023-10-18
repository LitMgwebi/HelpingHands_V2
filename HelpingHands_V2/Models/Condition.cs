using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HelpingHands_V2.Models;

public partial class Condition
{
    public int ConditionId { get; set; }

    [DisplayName("Condition Name")]
    public string ConditionName { get; set; } = null!;

    [DisplayName("Condition Description")]
    public string ConditionDescription { get; set; } = null!;

    public bool Active { get; set; } = true;

    public virtual ICollection<PatientCondition> PatientConditions { get; set; } = new List<PatientCondition>();
}
