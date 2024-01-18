using OptimizingLastMile.Models.Response.Statistics;

namespace OptimizingLastMile.Services.Statistics;

public interface IStatisticService
{
    Task<List<ManagerStatisticOrderResponse>> GetStatisticOrderEachMonthOfMananager(long managerId, DateTime startTimeParam, DateTime endTimeParam);
}