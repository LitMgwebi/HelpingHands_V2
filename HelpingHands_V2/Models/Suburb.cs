using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HelpingHands_V2.Models;

public partial class Suburb
{
    public int SuburbId { get; set; }

    [DisplayName("Suburb")]
    [Required(ErrorMessage ="Please enter the name of the suburb.")]
    [DataType(DataType.Text)]
    public string SuburbName { get; set; } = null!;

    [DisplayName("Postal Code")]
    [Required(ErrorMessage = "Please enter the postal code of the suburb.")]
    [DataType(DataType.Text)]
    public int? PostalCode { get; set; }

    [DisplayName("City")]
    [Required(ErrorMessage = "Please choose a city. If you can't find city, please add it.")]
    public int? CityId { get; set; }

    public bool Active { get; set; } = true;

    public virtual ICollection<BusinessInformation> BusinessInformations { get; set; } = new List<BusinessInformation>();

    public virtual ICollection<CareContract> CareContracts { get; set; } = new List<CareContract>();

    public virtual City City { get; set; } = null!;

    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();

    public virtual ICollection<PrefferedSuburb> PrefferedSuburbs { get; set; } = new List<PrefferedSuburb>();
}
