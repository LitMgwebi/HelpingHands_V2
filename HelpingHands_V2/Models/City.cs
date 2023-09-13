using System;
using System.Collections.Generic;

namespace HelpingHands_V2.Models;

public partial class City
{
    public int CityId { get; set; }

    public string CityName { get; set; } = null!;

    public string CityAbbreviation { get; set; } = null!;

    public bool Active { get; set; }

    public virtual ICollection<Suburb> Suburbs { get; set; } = new List<Suburb>();
}
