﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HelpingHands_V2.Models;

public partial class OperationHour
{
    public int OperationHoursId { get; set; }

    [Display(Name = "Day of Operation")]
    [DataType(DataType.Text)]
    public string? OperationDay { get; set; }

    [Display(Name = "Opening Time")]
    [DataType(DataType.Time)]
    public TimeSpan? OpenTime { get; set; }

    [Display(Name = "Closing Time")]
    [DataType(DataType.Time)]
    public TimeSpan? CloseTime { get; set; }

    public bool Active { get; set; } = true;

    public int? BusinessId { get; set; } = 1;

    public virtual BusinessInformation? Business { get; set; }
}
