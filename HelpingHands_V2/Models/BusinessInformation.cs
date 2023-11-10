using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HelpingHands_V2.Models;

public partial class BusinessInformation
{
    public int BusinessId { get; set; }

    [DisplayName("Organization Name")]
    [Required(ErrorMessage = "Please enter the name of the organization")]
    public string? OrganizationName { get; set; }

    [DisplayName("NPO Number")]
    [Required(ErrorMessage = "Please enter business NPO number")]
    public string? Nponumber { get; set; }

    [DisplayName("Address Line 1")]
    [DataType(DataType.MultilineText)]
    [Required(ErrorMessage = "Please enter the 1st address line")]
    public string? AddressLineOne { get; set; }

    [DisplayName("Address Line 2")]
    [DataType(DataType.MultilineText)]
    public string? AddressLineTwo { get; set; }

    [DisplayName("Suburb")]
    [Required(ErrorMessage = "Please select a suburb.")]
    public int? SuburbId { get; set; }

    [DisplayName("Contract Number")]
    [Required(ErrorMessage = "Please enter a contact number")]
    [Phone(ErrorMessage ="Please enter a valid contact number")]
    [DataType(DataType.PhoneNumber)]
    public string? ContactNumber { get; set; }

    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Please enter an email address")]
    [EmailAddress(ErrorMessage ="Please enter a valid email address")]
    public string? Email { get; set; }

    public bool Active { get; set; } = true;

    public virtual ICollection<OperationHour> OperationHours { get; set; } = new List<OperationHour>();

    public virtual Suburb? Suburb { get; set; }
}
