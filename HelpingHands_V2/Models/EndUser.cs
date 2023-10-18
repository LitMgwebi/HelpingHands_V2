using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HelpingHands_V2.Models;

public partial class EndUser
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    [DisplayName("First Name")]
    public string Firstname { get; set; } = null!;

    [Display(Name = "Last Name")]
    public string Lastname { get; set; } = null!;

    [DisplayName("Date of Birth")]
    public DateTime DateOfBirth { get; set; }

    [EmailAddress(ErrorMessage = "Please enter the correct email")]
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Gender { get; set; } = null!;

    [DisplayName("Contact Number")]
    [Phone(ErrorMessage = "Please enter the contact number.")]
    public string ContactNumber { get; set; } = null!;

    [DisplayName("ID Number")]
    public string? Idnumber { get; set; }

    [DisplayName("Display Name")]
    public string? UserType { get; set; }

    [DisplayName("Application Type")]
    public string? ApplicationType { get; set; }

    [DisplayName("Profile Picture")]
    public byte[]? ProfilePicture { get; set; }

    public string? ProfilePictureName { get; set; }

    public bool Active { get; set; } = true;

    public virtual Nurse? Nurse { get; set; }

    public virtual Patient? Patient { get; set; }
}
