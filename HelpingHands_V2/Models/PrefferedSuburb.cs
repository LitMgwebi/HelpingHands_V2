using System;
using System.Collections.Generic;

namespace HelpingHands_V2.Models;

public partial class PrefferedSuburb
{
    public int NurseId { get; set; }

    public int SuburbId { get; set; }

    public bool Active { get; set; }

    public virtual Nurse Nurse { get; set; } = null!;

    public virtual Suburb Suburb { get; set; } = null!;
}
