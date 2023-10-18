using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HelpingHands_V2.Models;

public partial class City
{
    public int CityId { get; set; }

    [Display(Name = "City Name")]
    public string CityName { get; set; } = null!;

    [DisplayName("City Abbreviation")]
    public string CityAbbreviation { get; set; } = null!;

    public bool Active { get; set; } = true;

    public virtual ICollection<Suburb> Suburbs { get; set; } = new List<Suburb>();
}
