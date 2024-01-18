using System;
using System.ComponentModel.DataAnnotations;

namespace OptimizingLastMile.Models.Params.Statistics;

public class ManagerOrderParam
{
    [Required]
    public DateTime? StartTime { get; set; }

    [Required]
    public DateTime? EndTime { get; set; }
}