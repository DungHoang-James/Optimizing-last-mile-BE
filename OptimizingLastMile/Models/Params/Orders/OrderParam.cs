using System;
using OptimizingLastMile.Entites.Enums;

namespace OptimizingLastMile.Models.Params.Orders;

public class OrderParam : ResourceParam
{
    public string? SearchName { get; set; }
    public string? SearchOrderId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<OrderStatusEnum>? Status { get; set; }
    public string? Sort { get; set; }
}