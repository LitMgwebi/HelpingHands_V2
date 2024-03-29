﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HelpingHands_V2.Models;

public partial class CareContract
{
    public int ContractId { get; set; }

    [DisplayName("Contract Status")]
    public string? ContractStatus { get; set; }

    [DisplayName("Contract Date")]
    [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}")]
    [DataType(DataType.Date, ErrorMessage = "Please enter a valid date")]
    [Required(ErrorMessage ="Please enter the contract's date")]
    public DateTime ContractDate { get; set; }

    [DisplayName("Patient")]
    public int PatientId { get; set; }

    [DisplayName("Nurse")]
    public int? NurseId { get; set; } = null;

    [DisplayName("Wound")]
    [Required(ErrorMessage = "Please select the type of wound you have.")]
    public int? WoundId { get; set; }

    [DataType(DataType.MultilineText)]
    [DisplayName("Address Line 1")]
    [Required(ErrorMessage = "Please enter the 1st address line")]
    public string? AddressLineOne { get; set; }

    [DataType(DataType.MultilineText)]
    [DisplayName("Address Line 2")]
    public string? AddressLineTwo { get; set; } = null;

    [DisplayName("Suburb")]
    [Required(ErrorMessage = "Please select a suburb.")]
    public int? SuburbId { get; set; }

    [DisplayName("Start Date")]
    [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date, ErrorMessage ="Please enter a valid date")]
    public DateTime? StartDate { get; set; } = null;

    [DisplayName("End Date")]
    [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date, ErrorMessage = "Please enter a valid date")]
    public DateTime? EndDate { get; set; } = null;

    [DataType(DataType.MultilineText)]
    [DisplayName("Wound Description")]
    public string? ContractComment { get; set; }

    public bool Active { get; set; } = true;

    public virtual EndUser? Nurse { get; set; }

    public virtual EndUser? Patient { get; set; }

    public virtual Suburb? Suburb { get; set; }

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();

    public virtual Wound? Wound { get; set; }
}
