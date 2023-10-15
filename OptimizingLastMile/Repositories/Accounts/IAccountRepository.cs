using System;
using OptimizingLastMile.Entites;
using OptimizingLastMile.Entites.Enums;
using OptimizingLastMile.Repositories.Base;
using OptimizingLastMile.Utils;

namespace OptimizingLastMile.Repositories.Accounts;

public interface IAccountRepository : IBaseRepository<Account>
{
    Task<Account> GetByUsername(string username);
    Task<Account> GetByIdIncludeProfile(long id);
    Task<Pagination<Account>> GetPaginationAccountIncludeProfile(string name, RoleEnum role, int pageNumber, int pageSize);
    Task<Account> GetAccountIncludeOrderShipping(long id);
}

