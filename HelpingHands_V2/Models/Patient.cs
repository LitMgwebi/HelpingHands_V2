using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HelpingHands_V2.Models;

public partial class Patient
{
    public int PatientId { get; set; }

    [DataType(DataType.MultilineText)]
    [DisplayName("Address Line 1")]
    public string? AddressLineOne { get; set; }

    [DataType(DataType.MultilineText)]
    [DisplayName("Address Line 2")]
    public string? AddressLineTwo { get; set; }

    [Display(Name = "Suburb")]
    public int? SuburbId { get; set; }

    [Display(Name = "Emergency Contact Name")]
    public string Icename { get; set; } = null!;

    [Display(Name = "Emergency Contact Number")]
    [Required(ErrorMessage = "Please enter a contact number")]
    [Phone(ErrorMessage = "Please enter a valid contact number")]
    [DataType(DataType.PhoneNumber)]
    public string Icenumber { get; set; } = null!;

    [Display(Name = "Additional Information")]
    [DataType(DataType.MultilineText)]
    public string? AdditionalInfo { get; set; }

    public bool Active { get; set; } = true;

    public virtual ICollection<CareContract> CareContracts { get; set; } = new List<CareContract>();

    public virtual ICollection<PatientCondition> PatientConditions { get; set; } = new List<PatientCondition>();

    public virtual EndUser PatientNavigation { get; set; } = null!;

    public virtual Suburb? Suburb { get; set; }
}
