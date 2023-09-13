using System;
using System.Collections.Generic;

namespace HelpingHands_V2.Models;

public partial class EndUsers
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public string? Idnumber { get; set; }

    public string? UserType { get; set; }

    public string? ApplicationType { get; set; }

    public byte[]? ProfilePicture { get; set; }

    public string? ProfilePictureName { get; set; }

    public bool Active { get; set; }

    public virtual Nurse? Nurse { get; set; }

    public virtual Patient? Patient { get; set; }
}
