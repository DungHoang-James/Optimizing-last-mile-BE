using System;
using OptimizingLastMile.Entites;
using OptimizingLastMile.Entites.Enums;
using OptimizingLastMile.Models.Commons;
using OptimizingLastMile.Repositories.Accounts;

namespace OptimizingLastMile.Services.Accounts;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        this._accountRepository = accountRepository;
    }

    public async Task<Account> GetByUsername(string username)
    {
        return await _accountRepository.GetByUsername(username.Trim());
    }

    public async Task<GenericResult<Account>> CreateManagerAcc(string username,
        string password,
        string name,
        DateOnly? birthDay,
        string province,
        string district,
        string ward,
        string address,
        string phoneContact)
    {
        var acc = await GetByUsername(username);

        if (acc is not null)
        {
            var error = Errors.Auth.UsernameAlreadyExist();
            return GenericResult<Account>.Fail(error);
        }

        var passEncrypt = BCrypt.Net.BCrypt.HashPassword(password.Trim());

        var newAcc = new Account(username, passEncrypt, RoleEnum.MANAGER, StatusEnum.ACTIVE);
        var createProfileResult = AccountProfile.Create(name, birthDay, province, district, ward, address, phoneContact);

        if (createProfileResult.IsFail)
        {
            return GenericResult<Account>.Fail(createProfileResult.Error);
        }

        newAcc.AccountProfile = createProfileResult.Data;

        _accountRepository.Create(newAcc);
        await _accountRepository.SaveAsync();

        return GenericResult<Account>.Ok(newAcc);
    }

    public async Task<GenericResult<Account>> RegisterByUsername(string username, string password, RoleEnum role)
    {
        var account = await GetByUsername(username);

        if (account is not null)
        {
            var error = Errors.Auth.UsernameAlreadyExist();
            return GenericResult<Account>.Fail(error);
        }

        if (role != RoleEnum.CUSTOMER && role != RoleEnum.DRIVER)
        {
            var error = Errors.Auth.RoleNotAllowRegisterByUsername();
            return GenericResult<Account>.Fail(error);
        }

        var passEncrypt = BCrypt.Net.BCrypt.HashPassword(password);

        var newAcc = new Account(username, passEncrypt, role, StatusEnum.NEW);

        _accountRepository.Create(newAcc);
        await _accountRepository.SaveAsync();

        return GenericResult<Account>.Ok(newAcc);
    }
}

