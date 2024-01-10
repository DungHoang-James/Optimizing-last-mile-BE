using System;
using OptimizingLastMile.Models.Commons;

namespace OptimizingLastMile.Models.Response.Orders;

public class OrderCreateBatchResponse
{
    public int No { get; set; }
    public Guid? OrderId { get; set; }
    public bool IsSuccess { get; set; }
    public ErrorObject Error { get; set; }
}