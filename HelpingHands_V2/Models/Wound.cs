using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HelpingHands_V2.Models;

public partial class Wound
{
    public int WoundId { get; set; }

    [DisplayName("Wound Name")]
    [Required]
    public string WoundName { get; set; } = null!;

    public string? Grade { get; set; }

    [DisplayName("Wound Description")]
    public string? WoundDescription { get; set; }

    public bool Active { get; set; } = true;

    public virtual ICollection<CareContract> CareContracts { get; set; } = new List<CareContract>();
} 
