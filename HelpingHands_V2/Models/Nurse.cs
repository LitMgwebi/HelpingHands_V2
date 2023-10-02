using System;
using System.Collections.Generic;

namespace HelpingHands_V2.Models;

public partial class Nurse
{
    public int NurseId { get; set; }

    public string Grade { get; set; } = null!;

    public bool Active { get; set; }

    public virtual ICollection<CareContract> CareContracts { get; set; } = new List<CareContract>();

    public virtual EndUser NurseNavigation { get; set; } = null!;

    public virtual ICollection<PrefferedSuburb> PrefferedSuburbs { get; set; } = new List<PrefferedSuburb>();
}
