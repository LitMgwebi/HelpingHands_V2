using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HelpingHands_V2.Models;

public partial class Nurse
{
    public int NurseId { get; set; }

    [Required(ErrorMessage = "Please enter your grade level")]
    public string Grade { get; set; } = null!;

    public bool Active { get; set; } = true;

    public virtual ICollection<CareContract> CareContracts { get; set; } = new List<CareContract>();

    public virtual EndUser? EndUser { get; set; }
}
