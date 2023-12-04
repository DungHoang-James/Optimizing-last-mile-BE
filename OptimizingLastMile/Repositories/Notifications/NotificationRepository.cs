using Microsoft.EntityFrameworkCore;
using OptimizingLastMile.Configs;
using OptimizingLastMile.Entites;
using OptimizingLastMile.Repositories.Base;
using OptimizingLastMile.Utils;

namespace OptimizingLastMile.Repositories.Notifications;

public class NotificationRepository : BaseRepository<NotificationLog>, INotificationRepository
{
    private readonly OlmDbContext _dbContext;

    public NotificationRepository(OlmDbContext dbContext) : base(dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<Pagination<NotificationLog>> GetNotificationPaging(long receiverId, int pageNumber, int pageSize)
    {
        var query = _dbContext.NotificationLogs
            .Include(n => n.Driver).ThenInclude(a => a.DriverProfile)
            .Include(n => n.Receiver)
            .Where(n => n.ReceiverId == receiverId)
            .OrderByDescending(n => n.CreatedDate);

        return await Pagination<NotificationLog>.CreateAsync(query, pageNumber, pageSize);
    }
}