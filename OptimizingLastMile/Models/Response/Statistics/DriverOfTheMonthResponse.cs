using System;
namespace OptimizingLastMile.Models.Response.Statistics;

public class DriverOfTheMonthResponse
{
    public long DriverId { get; set; }
    public string DriverName { get; set; }
    public string AvatarUrl { get; set; }
    public int TotalOrderDeliverySuccess { get; set; }
}