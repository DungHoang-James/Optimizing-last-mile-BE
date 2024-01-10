namespace OptimizingLastMile.Models.Requests.Orders;

public class OrderCreateBatchPayload
{
    public int No { get; set; }
    public OrderCreatePayload Order { get; set; }
}