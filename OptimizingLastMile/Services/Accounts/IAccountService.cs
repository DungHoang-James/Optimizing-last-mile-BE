using System;
using OptimizingLastMile.Entites;
using OptimizingLastMile.Entites.Enums;
using OptimizingLastMile.Models.Commons;

namespace OptimizingLastMile.Services.Accounts;

public interface IAccountService
{
    Task<Account> GetByUsername(string username);
    Task<GenericResult<Account>> CreateManagerAcc(string username,
        string password,
        string name,
        DateOnly? birthDay,
        string province,
        string district,
        string ward,
        string address,
        string phoneContact);
    Task<GenericResult<Account>> RegisterByUsername(string username, string password, RoleEnum role);
}

