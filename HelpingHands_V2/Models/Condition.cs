using System;
using System.Collections.Generic;

namespace HelpingHands_V2.Models;

public partial class Condition
{
    public int ConditionId { get; set; }

    public string ConditionName { get; set; } = null!;

    public string ConditionDescription { get; set; } = null!;

    public bool Active { get; set; }

    public virtual ICollection<PatientCondition> PatientConditions { get; set; } = new List<PatientCondition>();
}
