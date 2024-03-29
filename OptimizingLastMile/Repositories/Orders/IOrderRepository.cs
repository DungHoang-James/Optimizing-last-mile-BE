﻿using System;
using OptimizingLastMile.Entites;
using OptimizingLastMile.Entites.Enums;
using OptimizingLastMile.Repositories.Base;
using OptimizingLastMile.Utils;

namespace OptimizingLastMile.Repositories.Orders;

public interface IOrderRepository : IBaseRepository<OrderInformation>
{
    Task<OrderInformation> GetOrderDetail(Guid id);
    Task<OrderInformation> GetOrderIncludeAudit(Guid id);
    Task<Pagination<OrderInformation>> GetOrderForDriver(
        long driverId,
        DateTime? startDate,
        DateTime? endDate,
        List<OrderStatusEnum> orderStatus,
        string sort,
        int pageSize,
        int pageNumber);
    Task<Pagination<OrderInformation>> GetOrderForCustomer(
        long customerId,
        DateTime? startDate,
        DateTime? endDate,
        List<OrderStatusEnum> orderStatus,
        string sort,
        int pageSize,
        int pageNumber);
    Task<Pagination<OrderInformation>> GetOrderForManager(
        long managerId,
        string searchName,
        string searchOrderId,
        DateTime? startDate,
        DateTime? endDate,
        List<OrderStatusEnum> orderStatus,
        string sort,
        int pageSize,
        int pageNumber);
    Task<List<OrderInformation>> GetOrderShippingInDay(long driverId);
    Task<List<OrderInformation>> GetAllOrderFromAndToDate(long managerId, DateTime startTime, DateTime endTime);
    Task<List<OrderInformation>> GetOrderShippingInDayNotHaveDriver();
}

