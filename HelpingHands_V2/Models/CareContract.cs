using System;
using System.Collections.Generic;

namespace HelpingHands_V2.Models;

public partial class CareContract
{
    public int ContractId { get; set; }

    public string ContractStatus { get; set; } = null!;

    public DateTime ContractDate { get; set; }

    public int PatientId { get; set; }

    public int? NurseId { get; set; }

    public int? WoundId { get; set; }

    public string? AddressLineOne { get; set; }

    public string? AddressLineTwo { get; set; }

    public int? SuburbId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? ContractComment { get; set; }

    public bool Active { get; set; }

    public virtual Nurse? Nurse { get; set; }

    public virtual Patient Patient { get; set; } = null!;

    public virtual Suburb? Suburb { get; set; }

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();

    public virtual Wound? Wound { get; set; }
}
