using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HelpingHands_V2.Models;

public partial class Condition
{
    public int ConditionId { get; set; }

    [DisplayName("Condition Name")]
    [Required(ErrorMessage ="Please enter the name of the condition")]
    [MinLength(2, ErrorMessage ="Condition name has to be more than 2 characters")]
    public string ConditionName { get; set; } = null!;

    [DisplayName("Description")]
    [Required(ErrorMessage ="Please enter the description of the condition")]
    [DataType(DataType.MultilineText)]
    public string ConditionDescription { get; set; } = null!;

    public bool Active { get; set; } = true;

    public virtual ICollection<PatientCondition> PatientConditions { get; set; } = new List<PatientCondition>();
}
