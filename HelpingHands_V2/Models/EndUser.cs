using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace HelpingHands_V2.Models;

public partial class EndUser
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "Please enter a username")]
    public string Username
    {
        get
        {
            return Firstname.Length > 4 && Lastname.Length > 4  ? Firstname.Substring(0, 3) + Lastname.Substring(0, 3): "";
        }
    }

    [Required(ErrorMessage = "Please enter your first name")]
    [DisplayName("First Name")]
    public string Firstname { get; set; } = null!;

    [Required(ErrorMessage = "Please enter your last name")]
    [Display(Name = "Last Name")]
    public string Lastname { get; set; } = null!;

    [Required(ErrorMessage = "Please enter your date of birth")]
    [DisplayName("Date of Birth")]
    [DisplayFormat(DataFormatString = "{0:dd:MM:yyyy}")]
    [DataType(DataType.Date, ErrorMessage = "Please enter a valid date")]
    public DateTime DateOfBirth { get; set; }

    [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address")]
    [Required(ErrorMessage = "Please enter an email address")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Please enter your password")]
    [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Please confirm your password")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string? ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Please enter your gender")]
    public string Gender { get; set; } = null!;

    [DisplayName("Contract Number")]
    [Required(ErrorMessage = "Please enter a contact number")]
    [Phone(ErrorMessage = "Please enter a valid contact number")]
    public string ContactNumber { get; set; } = null!;

    [Required(ErrorMessage = "Please enter your ID Number")]
    [DisplayName("ID Number")]
    public string? Idnumber { get; set; }

    [Required(ErrorMessage = "Please select the user's type")]
    [DisplayName("User Type")]
    public string? UserType { get; set; }

    [DisplayName("Application Type")]
    public string? ApplicationType { get; set; } = null;

    [DisplayName("Profile Picture")]
    public string? ProfilePicture { get; set; } = null;

    public string? ProfilePictureName { get; set; } = null;

    public bool Active { get; set; } = true;

    [Display(Name = "Full Name")]
    public string FullName
    {
        get
        {
            return Firstname + " " + Lastname;
        }
    }

    public virtual Nurse? Nurse { get; set; }
    public virtual Patient? Patient { get; set; }
    public virtual ICollection<PatientCondition> PatientConditions { get; set; } = new List<PatientCondition>();
    public virtual ICollection<PrefferedSuburb> PrefferedSuburbs { get; set; } = new List<PrefferedSuburb>();
    public virtual ICollection<CareContract> CareContracts { get; set; } = new List<CareContract>();
}
