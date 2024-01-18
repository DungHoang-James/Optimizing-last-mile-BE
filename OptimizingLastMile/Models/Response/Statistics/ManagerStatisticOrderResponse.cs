using System;
namespace OptimizingLastMile.Models.Response.Statistics;

public class ManagerStatisticOrderResponse
{
    public int Month { get; set; }
    public int Year { get; set; }
    public string MonthName { get; set; }
    public int TotalOrder { get; set; }
    public int NumberOfOrderProcessing { get; set; }
    public int NumberOfOrderDeliverySuccess { get; set; }
    public int NumberOfOrderDeliveryFailed { get; set; }
    public int NumberOfOrderDeleted { get; set; }
}