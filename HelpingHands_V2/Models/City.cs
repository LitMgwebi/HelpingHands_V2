using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HelpingHands_V2.Models;

public partial class City
{
    public int CityId { get; set; }

    [Display(Name = "Name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage ="Please enter the name of the city.")]
    public string CityName { get; set; } = null!;

    [DisplayName("Abbreviation")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage ="Please enter the abbreviation of the city.")]
    public string CityAbbreviation { get; set; } = null!;

    public bool Active { get; set; } = true;

    public virtual ICollection<Suburb> Suburbs { get; set; } = new List<Suburb>();
}
