using System;
using Microsoft.EntityFrameworkCore;
using OptimizingLastMile.Configs;
using OptimizingLastMile.Entites;
using OptimizingLastMile.Entites.Enums;
using OptimizingLastMile.Models.Params.Orders;
using OptimizingLastMile.Repositories.Base;
using OptimizingLastMile.Services.Others;
using OptimizingLastMile.Utils;

namespace OptimizingLastMile.Repositories.Orders;

public class OrderRepository : BaseRepository<OrderInformation>, IOrderRepository
{
    private readonly OlmDbContext _dbContext;
    private readonly IPropertyMappingService _propertyMappingService;

    public OrderRepository(OlmDbContext dbContext,
        IPropertyMappingService propertyMappingService) : base(dbContext)
    {
        this._dbContext = dbContext;
        this._propertyMappingService = propertyMappingService;
    }

    public async Task<OrderInformation> GetOrderDetail(Guid id)
    {
        return await _dbContext.OrderInformation
            .Include(o => o.Driver).ThenInclude(a => a.DriverProfile)
            .Include(o => o.Owner).ThenInclude(a => a.AccountProfile)
            .Include(o => o.Feedbacks)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<OrderInformation> GetOrderIncludeAudit(Guid id)
    {
        return await _dbContext.OrderInformation
            .Include(o => o.OrderAudits)
            .Include(o => o.Driver).ThenInclude(a => a.DriverProfile)
            .Include(o => o.Owner).ThenInclude(a => a.AccountProfile)
            .Include(o => o.Creator).ThenInclude(a => a.AccountProfile)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Pagination<OrderInformation>> GetOrderForManager(
        long managerId,
        string searchName,
        string searchOrderId,
        DateTime? startDate,
        DateTime? endDate,
        List<OrderStatusEnum> orderStatus,
        string sort,
        int pageSize,
        int pageNumber)
    {
        var query = _dbContext.OrderInformation
            .Include(o => o.Owner).ThenInclude(a => a.AccountProfile)
            .Include(o => o.Driver).ThenInclude(a => a.DriverProfile)
            .Include(o => o.Feedbacks)
            .Where(o => o.CreatorId == managerId);

        //if (!string.IsNullOrEmpty(searchName))
        //{
        //    query = query.Where(o => o.Driver.DriverProfile.Name.Contains(searchName) ||
        //    o.Owner.AccountProfile.Name.Contains(searchName) ||
        //    o.RecipientName.Contains(searchName) ||
        //    o.SenderName.Contains(searchName));
        //}

        if (!string.IsNullOrEmpty(searchOrderId))
        {
            query = query.Where(o => o.Id.ToString().Contains(searchOrderId));
        }

        if (startDate.HasValue && endDate.HasValue)
        {
            query = query.Where(o => o.ExpectedShippingDate.HasValue &&
            o.ExpectedShippingDate.Value >= startDate.Value &&
            o.ExpectedShippingDate.Value <= endDate.Value);
        }
        else if (startDate.HasValue)
        {
            query = query.Where(o => o.ExpectedShippingDate.HasValue && o.ExpectedShippingDate.Value >= startDate.Value);
        }
        else if (endDate.HasValue)
        {
            query = query.Where(o => o.ExpectedShippingDate.HasValue && o.ExpectedShippingDate.Value <= endDate.Value);
        }

        if (orderStatus is not null && orderStatus.Count > 0)
        {
            query = query.Where(o => orderStatus.Contains(o.CurrentOrderStatus));
        }

        if (!string.IsNullOrWhiteSpace(sort))
        {
            var propertyMapDic = _propertyMappingService.GetPropertyMapping<OrderParam, OrderInformation>();
            query = query.ApplySort(sort, propertyMapDic);
        }

        return await Pagination<OrderInformation>.CreateAsync(query, pageNumber, pageSize);
    }

    public async Task<Pagination<OrderInformation>> GetOrderForCustomer(
        long customerId,
        DateTime? startDate,
        DateTime? endDate,
        List<OrderStatusEnum> orderStatus,
        string sort,
        int pageSize,
        int pageNumber)
    {
        var query = _dbContext.OrderInformation
            .Include(o => o.Owner).ThenInclude(a => a.AccountProfile)
            .Include(o => o.Driver).ThenInclude(a => a.DriverProfile)
            .Include(o => o.Feedbacks)
            .Where(o => o.OwnerId == customerId && o.CurrentOrderStatus != OrderStatusEnum.DELETED);

        if (startDate.HasValue && endDate.HasValue)
        {
            query = query.Where(o => o.ExpectedShippingDate.HasValue &&
            o.ExpectedShippingDate.Value >= startDate.Value &&
            o.ExpectedShippingDate.Value <= endDate.Value);
        }
        else if (startDate.HasValue)
        {
            query = query.Where(o => o.ExpectedShippingDate.HasValue && o.ExpectedShippingDate.Value >= startDate.Value);
        }
        else if (endDate.HasValue)
        {
            query = query.Where(o => o.ExpectedShippingDate.HasValue && o.ExpectedShippingDate.Value <= endDate.Value);
        }

        if (orderStatus is not null && orderStatus.Count > 0)
        {
            query = query.Where(o => orderStatus.Contains(o.CurrentOrderStatus));
        }

        if (!string.IsNullOrWhiteSpace(sort))
        {
            var propertyMapDic = _propertyMappingService.GetPropertyMapping<OrderParam, OrderInformation>();
            query = query.ApplySort(sort, propertyMapDic);
        }

        return await Pagination<OrderInformation>.CreateAsync(query, pageNumber, pageSize);
    }

    public async Task<Pagination<OrderInformation>> GetOrderForDriver(
        long driverId,
        DateTime? startDate,
        DateTime? endDate,
        List<OrderStatusEnum> orderStatus,
        string sort,
        int pageSize,
        int pageNumber)
    {
        var query = _dbContext.OrderInformation
            .Include(o => o.Owner).ThenInclude(a => a.AccountProfile)
            .Include(o => o.Driver).ThenInclude(a => a.DriverProfile)
            .Include(o => o.Feedbacks)
            .Where(o => o.DriverId == driverId && o.CurrentOrderStatus != OrderStatusEnum.DELETED);

        if (startDate.HasValue && endDate.HasValue)
        {
            query = query.Where(o => o.ExpectedShippingDate.HasValue &&
            o.ExpectedShippingDate.Value >= startDate.Value &&
            o.ExpectedShippingDate.Value <= endDate.Value);
        }
        else if (startDate.HasValue)
        {
            query = query.Where(o => o.ExpectedShippingDate.HasValue && o.ExpectedShippingDate.Value >= startDate.Value);
        }
        else if (endDate.HasValue)
        {
            query = query.Where(o => o.ExpectedShippingDate.HasValue && o.ExpectedShippingDate.Value <= endDate.Value);
        }

        if (orderStatus is not null && orderStatus.Count > 0)
        {
            query = query.Where(o => orderStatus.Contains(o.CurrentOrderStatus));
        }

        if (!string.IsNullOrWhiteSpace(sort))
        {
            var propertyMapDic = _propertyMappingService.GetPropertyMapping<OrderParam, OrderInformation>();
            query = query.ApplySort(sort, propertyMapDic);
        }

        return await Pagination<OrderInformation>.CreateAsync(query, pageNumber, pageSize);
    }

    public async Task<List<OrderInformation>> GetOrderShippingInDay(long driverId)
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        return await _dbContext.OrderInformation
            .Include(o => o.Owner).ThenInclude(a => a.AccountProfile)
            .Include(o => o.Driver).ThenInclude(a => a.DriverProfile)
            .Where(o => o.DriverId == driverId &&
        (o.ExpectedShippingDate >= today && o.ExpectedShippingDate <= tomorrow)).ToListAsync();
    }

    public Task<List<OrderInformation>> GetAllOrderFromAndToDate(long managerId, DateTime startTime, DateTime endTime)
    {
        return _dbContext.OrderInformation.Where(o => o.CreatorId == managerId &&
        o.CreatedAt >= startTime && o.CreatedAt <= endTime).ToListAsync();
    }

    public Task<List<OrderInformation>> GetOrderShippingInDayNotHaveDriver()
    {
        var today = DateTime.Today;

        return _dbContext.OrderInformation
            .Include(o => o.OrderAudits)
            .Where(o => o.ExpectedShippingDate.Value.Year == today.Year &&
        o.ExpectedShippingDate.Value.Month == today.Month &&
        o.ExpectedShippingDate.Value.Day == today.Day &&
        o.CurrentOrderStatus == OrderStatusEnum.CREATED &&
        o.DriverId == null).ToListAsync();
    }
}

