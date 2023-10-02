using System;
using System.Collections.Generic;

namespace HelpingHands_V2.Models;

public partial class Suburb
{
    public int SuburbId { get; set; }

    public string SuburbName { get; set; } = null!;

    public int PostalCode { get; set; }

    public int CityId { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<BusinessInformation> BusinessInformations { get; set; } = new List<BusinessInformation>();

    public virtual ICollection<CareContract> CareContracts { get; set; } = new List<CareContract>();

    public virtual City City { get; set; } = null!;

    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();

    public virtual ICollection<PrefferedSuburb> PrefferedSuburbs { get; set; } = new List<PrefferedSuburb>();
}
