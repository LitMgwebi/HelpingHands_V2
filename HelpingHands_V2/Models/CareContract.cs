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
    public DateTime ContractDate { get; set; }

    [DisplayName("Patient Name")]
    public int PatientId { get; set; }

    [DisplayName("Nurse Name")]
    public int? NurseId { get; set; }

    [DisplayName("Wound Name")]
    public int? WoundId { get; set; }

    [DisplayName("First Address Line")]
    public string? AddressLineOne { get; set; }

    [DisplayName("Second Address Line")]
    public string? AddressLineTwo { get; set; }

    [DisplayName("Suburb Name")]
    public int? SuburbId { get; set; }

    [DisplayName("Start Date")]
    public DateTime? StartDate { get; set; }

    [DisplayName("End Date")]
    public DateTime? EndDate { get; set; }

    [DisplayName("Contract Comment")]
    public string? ContractComment { get; set; }

    public bool Active { get; set; } = true;

    public virtual Nurse? Nurse { get; set; }

    public virtual Patient Patient { get; set; } = null!;

    public virtual Suburb? Suburb { get; set; }

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();

    public virtual Wound? Wound { get; set; }
}
