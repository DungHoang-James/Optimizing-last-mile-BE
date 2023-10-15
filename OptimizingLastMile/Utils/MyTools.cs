using System;
using System.Security.Claims;

namespace OptimizingLastMile.Utils;

public class MyTools
{
    public static long GetUserOfRequest(IEnumerable<Claim> claims)
    {
        var id = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

        return long.Parse(id);
    }

    public static string GetRoleOfAuthRequest(IEnumerable<Claim> claims)
    {
        var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;

        return role;
    }
}

