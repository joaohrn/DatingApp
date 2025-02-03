using System;
using System.Security.Claims;

namespace Api.Extensions;

public static class ClaimsPrincipleExtensions
{
    public static string GetUsername(this ClaimsPrincipal user)
    {
        var username = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if(username == null) throw new Exception("Could not find username from token");

        return username;
    }
}
