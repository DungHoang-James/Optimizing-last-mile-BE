using System;
using OptimizingLastMile.Configure;
using OptimizingLastMile.Repositories.Accounts;
using OptimizingLastMile.Repositories.Base;
using OptimizingLastMile.Services.Accounts;
using OptimizingLastMile.Services.Auths;

namespace OptimizingLastMile.Utils;

public static class RegisterDI
{
    public static void RegisterDIService(this IServiceCollection services, ConfigurationManager configuration)
    {
        // Config
        services.Configure<JwtConfig>(configuration.GetSection("Jwt"));

        // Service
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAccountService, AccountService>();

        // Repository
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        services.AddScoped<IAccountRepository, AccountRepository>();
    }
}

