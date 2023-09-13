using System;
using System.Collections.Generic;

namespace HelpingHands_V2.Models;

public partial class Patient
{
    public int PatientId { get; set; }

    public string? AddressLineOne { get; set; }

    public string? AddressLineTwo { get; set; }

    public int? SuburbId { get; set; }

    public string Icename { get; set; } = null!;

    public string Icenumber { get; set; } = null!;

    public string? AdditionalInfo { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<CareContract> CareContracts { get; set; } = new List<CareContract>();

    public virtual ICollection<PatientCondition> PatientConditions { get; set; } = new List<PatientCondition>();

    public virtual EndUsers PatientNavigation { get; set; } = null!;

    public virtual Suburb? Suburb { get; set; }
}
