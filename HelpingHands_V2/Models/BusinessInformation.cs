using System;
using System.Collections.Generic;

namespace HelpingHands_V2.Models;

public partial class BusinessInformation
{
    public int BusinessId { get; set; }

    public string? OrganizationName { get; set; }

    public string? Nponumber { get; set; }

    public string? AddressLineOne { get; set; }

    public string? AddressLineTwo { get; set; }

    public int? SuburbId { get; set; }

    public string? ContactNumber { get; set; }

    public string? Email { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<OperationHour> OperationHours { get; set; } = new List<OperationHour>();

    public virtual Suburb? Suburb { get; set; }
}
