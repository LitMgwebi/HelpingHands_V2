using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HelpingHands_V2.Models;

public partial class OperationHour
{
    public int OperationHoursId { get; set; }

    [Display(Name = "Day of Operation")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Please enter the day of operation.")]
    public string? OperationDay { get; set; }

    [Display(Name = "Opening Time")]
    [DataType(DataType.Time)]
    [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
    [Required(ErrorMessage = "Please enter the opening time.")]
    public TimeSpan? OpenTime { get; set; }

    [Display(Name = "Closing Time")]
    [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Time)]
    [Required(ErrorMessage = "Please enter the closing time.")]
    public TimeSpan? CloseTime { get; set; }

    public bool Active { get; set; } = true;

    public int? BusinessId { get; set; } = 1;

    public virtual BusinessInformation? Business { get; set; }
}
