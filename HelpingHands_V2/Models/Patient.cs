using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HelpingHands_V2.Models;

public partial class Patient
{
    public int PatientId { get; set; }

    [Display(Name = "First Address Line")]
    public string? AddressLineOne { get; set; }

    [Display(Name = "Second Address Line")]
    public string? AddressLineTwo { get; set; }

    [Display(Name = "Suburb Name")]
    public int? SuburbId { get; set; }

    [Display(Name = "Emergency Contact Name")]
    public string Icename { get; set; } = null!;

    [Display(Name = "Emergency Contact Number")]
    [Phone(ErrorMessage = "Please enter correct phone number.")]
    public string Icenumber { get; set; } = null!;

    [Display(Name = "Additional Information")]
    public string? AdditionalInfo { get; set; }

    public bool Active { get; set; } = true;

    public virtual ICollection<CareContract> CareContracts { get; set; } = new List<CareContract>();

    public virtual ICollection<PatientCondition> PatientConditions { get; set; } = new List<PatientCondition>();

    public virtual EndUser PatientNavigation { get; set; } = null!;

    public virtual Suburb? Suburb { get; set; }
}
