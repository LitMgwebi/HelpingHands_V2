using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HelpingHands_V2.Models;

public partial class PrefferedSuburb
{
    [Display(Name = "Nurse")]
    public int? NurseId { get; set; }

    [Display(Name = "Suburb")]
    [Required(ErrorMessage = "Please select which suburb you want to add")]
    public int? SuburbId { get; set; }

    public bool Active { get; set; } = true;

    public virtual Nurse Nurse { get; set; } = null!;

    public virtual Suburb Suburb { get; set; } = null!;
}
