using System;
using System.Collections.Generic;

namespace HelpingHands_V2.Models;

public partial class OperationHour
{
    public int OperationHoursId { get; set; }

    public string? OperationDay { get; set; }

    public TimeSpan? OpenTime { get; set; }

    public TimeSpan? CloseTime { get; set; }

    public bool Active { get; set; }

    public int? BusinessId { get; set; }

    public virtual BusinessInformation? Business { get; set; }
}
