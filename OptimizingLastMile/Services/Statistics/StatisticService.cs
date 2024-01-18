using System;
using OptimizingLastMile.Entites.Enums;
using OptimizingLastMile.Models.Response.Statistics;
using OptimizingLastMile.Repositories.Orders;

namespace OptimizingLastMile.Services.Statistics;

public class StatisticService : IStatisticService
{
    private readonly Dictionary<int, string> MONTH = new()
    {
        {1, "January" },
        {2, "February" },
        {3, "March" },
        {4, "April" },
        {5, "May" },
        {6, "June" },
        {7, "July" },
        {8, "August" },
        {9, "September" },
        {10, "October" },
        {11, "November" },
        {12, "December" }
    };

    private readonly IOrderRepository _orderRepository;

    public StatisticService(IOrderRepository orderRepository)
    {
        this._orderRepository = orderRepository;
    }

    public async Task<List<ManagerStatisticOrderResponse>> GetStatisticOrderEachMonthOfMananager(long managerId, DateTime startTimeParam, DateTime endTimeParam)
    {
        var startDate = DateOnly.FromDateTime(startTimeParam);
        var endDate = DateOnly.FromDateTime(endTimeParam);

        var maxTime = TimeOnly.MaxValue;
        var minTime = TimeOnly.MinValue;

        var startTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, minTime.Hour, minTime.Minute, minTime.Second);
        var endTime = new DateTime(endDate.Year, endDate.Month, endDate.Day, maxTime.Hour, maxTime.Minute, maxTime.Second);

        var listOrder = await _orderRepository.GetAllOrderFromAndToDate(managerId, startTime, endTime);

        var result = new List<ManagerStatisticOrderResponse>();

        while (startDate <= endDate)
        {
            var orderInMonth = listOrder.Where(o => o.CreatedAt.Value.Year == startDate.Year &&
            o.CreatedAt.Value.Month == startDate.Month).ToList();

            var totalOrder = orderInMonth.Count;
            var totalOrderDeliverySuccess = orderInMonth.Count(o => o.CurrentOrderStatus == OrderStatusEnum.DELIVERED);
            var totalOrderDeliveryFailed = orderInMonth.Count(o => o.CurrentOrderStatus == OrderStatusEnum.DELIVERY_FAILED);
            var totalOrderDeleted = orderInMonth.Count(o => o.CurrentOrderStatus == OrderStatusEnum.DELETED);
            var totalOrderProcessing = orderInMonth.Count(o => o.CurrentOrderStatus != OrderStatusEnum.DELIVERED &&
            o.CurrentOrderStatus != OrderStatusEnum.DELIVERY_FAILED &&
            o.CurrentOrderStatus != OrderStatusEnum.DELETED);

            var managerStatisticOrder = new ManagerStatisticOrderResponse
            {
                Month = startDate.Month,
                MonthName = MONTH.GetValueOrDefault(startDate.Month),
                Year = startDate.Year,
                TotalOrder = totalOrder,
                NumberOfOrderDeliverySuccess = totalOrderDeliverySuccess,
                NumberOfOrderProcessing = totalOrderProcessing,
                NumberOfOrderDeliveryFailed = totalOrderDeliveryFailed,
                NumberOfOrderDeleted = totalOrderDeleted
            };

            result.Add(managerStatisticOrder);

            startDate = startDate.AddMonths(1);
        }

        return result;
    }
}