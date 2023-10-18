using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HelpingHands_V2.Models;

public partial class Suburb
{
    public int SuburbId { get; set; }

    [DisplayName("Suburb Name")]
    public string SuburbName { get; set; } = null!;

    [DisplayName("Postal Code")]
    public int PostalCode { get; set; }

    [DisplayName("City Name")]
    public int CityId { get; set; }

    public bool Active { get; set; } = true;

    public virtual ICollection<BusinessInformation> BusinessInformations { get; set; } = new List<BusinessInformation>();

    public virtual ICollection<CareContract> CareContracts { get; set; } = new List<CareContract>();

    public virtual City City { get; set; } = null!;

    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();

    public virtual ICollection<PrefferedSuburb> PrefferedSuburbs { get; set; } = new List<PrefferedSuburb>();
}
