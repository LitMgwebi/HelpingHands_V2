using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HelpingHands_V2.Models;

public partial class Wound
{
    public int WoundId { get; set; }

    [DisplayName("Wound Name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Please enter a the name of the wound")]
    public string WoundName { get; set; } = null!;

    [Required(ErrorMessage ="Please enter the grade level of the wound")]
    public string? Grade { get; set; }

    [DisplayName("Wound Description")]
    [DataType(DataType.MultilineText)]
    [Required(ErrorMessage = "Please enter a the description of the wound")]
    public string? WoundDescription { get; set; }

    public bool Active { get; set; } = true;

    public virtual ICollection<CareContract> CareContracts { get; set; } = new List<CareContract>();
} 
