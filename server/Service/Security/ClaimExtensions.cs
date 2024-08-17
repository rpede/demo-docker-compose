using System.Security.Claims;
using DataAccess.Entities;

namespace Service.Security;

public static class ClaimExtensions
{
    public static string GetUserId(this ClaimsPrincipal user) =>
        user.FindFirst(ClaimTypes.NameIdentifier)!.Value;

    public static void RequireRole(this ClaimsPrincipal user, params string[] roles)
    {
        if (!user.FindAll(ClaimTypes.Role).Any(x => roles.Contains(x.Value)))
        {
            throw new ForbiddenError();
        }
    }

    public static void RequireUserId(this ClaimsPrincipal user, string userId)
    {
        if (user.GetUserId() != userId)
        {
            throw new ForbiddenError();
        }
    }

    public static IEnumerable<Claim> ToClaims(this User user, IEnumerable<string> roles) =>
        [
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            // new(ClaimTypes.Email, user.Email!),
            .. roles.Select(role => new Claim(ClaimTypes.Role, role))
        ];

    public static IEnumerable<Claim> ToClaims(this User user, params string[] roles) =>
        ToClaims(user, roles.AsEnumerable());

    public static ClaimsPrincipal ToPrincipal(this User user, IEnumerable<string> roles) =>
        new ClaimsPrincipal(new ClaimsIdentity(user.ToClaims(roles)));

    public static ClaimsPrincipal ToPrincipal(this User user, params string[] roles) =>
        new ClaimsPrincipal(new ClaimsIdentity(user.ToClaims(roles.AsEnumerable())));
}