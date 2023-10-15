using System;
using OptimizingLastMile.Repositories.Base;
using OptimizingLastMile.Entites;
using OptimizingLastMile.Configs;
using Microsoft.EntityFrameworkCore;
using OptimizingLastMile.Entites.Enums;
using OptimizingLastMile.Utils;

namespace OptimizingLastMile.Repositories.Accounts;

public class AccountRepository : BaseRepository<Account>, IAccountRepository
{
    private readonly OlmDbContext _dbContext;

    public AccountRepository(OlmDbContext dbContext) : base(dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<Account> GetByUsername(string username)
    {
        return await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Username == username);
    }

    public async Task<Account> GetByIdIncludeProfile(long id)
    {
        return await _dbContext.Accounts
            .Include(a => a.AccountProfile)
            .Include(a => a.DriverProfile)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Pagination<Account>> GetPaginationAccountIncludeProfile(string name, RoleEnum role, int pageNumber, int pageSize)
    {
        var query = _dbContext.Accounts
            .Include(a => a.DriverProfile)
            .Include(a => a.AccountProfile)
            .Where(a => a.Role == role);

        if (!string.IsNullOrWhiteSpace(name))
        {
            if (role == RoleEnum.DRIVER)
            {
                query = query.Where(a => a.DriverProfile.Name.Contains(name));
            }
            else
            {
                query = query.Where(a => a.AccountProfile.Name.Contains(name));
            }
        }

        return await Pagination<Account>.CreateAsync(query, pageNumber, pageSize);
    }

    public async Task<Account> GetAccountIncludeOrderShipping(long id)
    {
        return await _dbContext.Accounts
            .Include(a => a.OrderReceived.Where(o => o.CurrentOrderStatus == OrderStatusEnum.SHIPPING))
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}

