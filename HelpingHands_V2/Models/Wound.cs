using System;
using System.Collections.Generic;

namespace HelpingHands_V2.Models;

public partial class Wound
{
    public int WoundId { get; set; }

    public string WoundName { get; set; } = null!;

    public string? Grade { get; set; }

    public string? WoundDescription { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<CareContract> CareContracts { get; set; } = new List<CareContract>();
}
