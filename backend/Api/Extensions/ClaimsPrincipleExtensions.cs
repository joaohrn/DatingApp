using System;
using System.Security.Claims;

namespace Api.Extensions;

public static class ClaimsPrincipleExtensions
{
    public static string GetUsername(this ClaimsPrincipal user)
    {
        var username = user.FindFirstValue(ClaimTypes.Name);
        if(username == null) throw new Exception("Could not find username from token");

        return username;
    }
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new Exception("Could not find username from token"));

        return userId;
    }
}
