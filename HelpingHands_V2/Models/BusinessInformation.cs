using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HelpingHands_V2.Models;

public partial class BusinessInformation
{
    public int BusinessId { get; set; }

    [DisplayName("Organization Name")]
    public string? OrganizationName { get; set; }

    [DisplayName("NPO Number")]
    public string? Nponumber { get; set; }

    [DisplayName("First Address Line")]
    public string? AddressLineOne { get; set; }

    [DisplayName("Second Address Line")]
    public string? AddressLineTwo { get; set; }

    [DisplayName("Suburb Name")]
    public int? SuburbId { get; set; }

    [DisplayName("Contract Number")]
    [Phone(ErrorMessage ="Please enter a phone number")]
    public string? ContactNumber { get; set; }

    [EmailAddress(ErrorMessage ="Please enter an email address")]
    public string? Email { get; set; }

    public bool Active { get; set; } = true;

    public virtual ICollection<OperationHour> OperationHours { get; set; } = new List<OperationHour>();

    public virtual Suburb? Suburb { get; set; }
}
