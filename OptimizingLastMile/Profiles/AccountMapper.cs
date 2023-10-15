using System;
using AutoMapper;
using OptimizingLastMile.Entites;
using OptimizingLastMile.Models.Response.AccountProfile;
using OptimizingLastMile.Models.Response.Managers;

namespace OptimizingLastMile.Profiles;

public class AccountMapper : Profile
{
    public AccountMapper()
    {
        CreateMap<Account, ManagerProfileResponse>();
        CreateMap<Account, ProfileDetailResponse>();
        CreateMap<Account, DriverProfileResponse>();
    }
}

