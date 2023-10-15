using System;
using OptimizingLastMile.Entites.Enums;
using OptimizingLastMile.Models.Commons;

namespace OptimizingLastMile.Entites;

public class Account
{
    public long Id { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Email { get; private set; }
    public string Username { get; private set; }
    public string Password { get; private set; }
    public RoleEnum Role { get; private set; }
    public StatusEnum Status { get; set; }

    public AccountProfile AccountProfile { get; set; }
    public DriverProfile DriverProfile { get; set; }

    public int CountOrderShipping
    {
        get => OrderReceived.Count(o => o.CurrentOrderStatus == OrderStatusEnum.SHIPPING);
    }

    public List<OrderInformation> OrderReceived { get; set; }

    protected Account() { }

    public Account(string username, string password, RoleEnum role, StatusEnum status)
    {
        Username = username.Trim();
        Password = password;
        Role = role;
        Status = status;
    }

    public GenericResult DeactiveDriver()
    {
        if (Role != RoleEnum.DRIVER)
        {
            var error = Errors.Common.MethodNotAllow();
            return GenericResult.Fail(error);
        }

        if (CountOrderShipping > 0)
        {
            var error = Errors.AccountProfile.NotDeactiveDriver();
            return GenericResult.Fail(error);
        }

        Status = StatusEnum.INACTIVE;
        return GenericResult.Ok();
    }
}

