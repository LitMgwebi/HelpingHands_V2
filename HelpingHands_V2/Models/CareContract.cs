using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HelpingHands_V2.Models;

public partial class CareContract
{
    public int ContractId { get; set; }

    [DisplayName("Contract Status")]
    public string ContractStatus { get; set; } = null!;

    [DisplayName("Contract Date")]
    [DisplayFormat(DataFormatString = "{0:dd:MM:yyyy}")]
    [DataType(DataType.Date, ErrorMessage = "Please enter a valid date")]
    [Required(ErrorMessage ="Please enter the contract's date")]
    public DateTime ContractDate { get; set; }

    [DisplayName("Patient")]
    public int PatientId { get; set; }

    [DisplayName("Nurse")]
    public int? NurseId { get; set; }

    [DisplayName("Wound")]
    public int? WoundId { get; set; }

    [DataType(DataType.MultilineText)]
    [DisplayName("Address Line 1")]
    [Required(ErrorMessage = "Please enter the address")]
    public string? AddressLineOne { get; set; }

    [DataType(DataType.MultilineText)]
    [DisplayName("Address Line 2")]
    public string? AddressLineTwo { get; set; }

    [DisplayName("Suburb")]
    public int? SuburbId { get; set; }

    [DisplayName("Start Date")]
    [DisplayFormat(DataFormatString = "{0:dd:MM:yyyy}")]
    [DataType(DataType.Date, ErrorMessage ="Please enter a valid date")]
    [Required(ErrorMessage = "Please enter the contract's starting date")]
    public DateTime? StartDate { get; set; }

    [DisplayName("End Date")]
    [DisplayFormat(DataFormatString = "{0:dd:MM:yyyy}")]
    [DataType(DataType.Date, ErrorMessage = "Please enter a valid date")]
    [Required(ErrorMessage = "Please enter the contract's ending date")]
    public DateTime? EndDate { get; set; }

    [DataType(DataType.MultilineText)]
    [DisplayName("Contract Comment")]
    public string? ContractComment { get; set; }

    public bool Active { get; set; } = true;

    public virtual Nurse? Nurse { get; set; }

    public virtual Patient Patient { get; set; } = null!;

    public virtual Suburb? Suburb { get; set; }

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();

    public virtual Wound? Wound { get; set; }
}
