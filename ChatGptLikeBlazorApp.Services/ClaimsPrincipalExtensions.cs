using System.Security.Claims;
using ChatGptLikeBlazorApp.Services.Interfaces.Data;

namespace ChatGptLikeBlazorApp.Services;
public static class ClaimsPrincipalExtensions
{
    public static OwnerInfo? ToOwnerInfo(this ClaimsIdentity claimsIdentity)
    {
        if (!claimsIdentity.IsAuthenticated)
        {
            return null;
        }

        var tenantId = GetTenantId(claimsIdentity);
        var objectId = GetObjectId(claimsIdentity);
        var displayName = claimsIdentity.FindFirst("name")?.Value ?? claimsIdentity.Name;
        if (tenantId is null || objectId is null || displayName is null)
        {
            return null;
        }

        return new()
        {
            DisplayName = displayName,
            TenantId = tenantId,
            ObjectId = objectId,
        };
    }

    private static string? GetObjectId(ClaimsIdentity claimsIdentity)
    {
        return claimsIdentity.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value
            ?? claimsIdentity.FindFirst("oid")?.Value;
    }

    private static string? GetTenantId(ClaimsIdentity claimsIdentity)
    {
        return claimsIdentity.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value ??
            claimsIdentity.FindFirst("tid")?.Value;
    }

    public static OwnerInfo? ToOwnerInfo(this ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal.Identity is ClaimsIdentity identity)
        {
            return identity.ToOwnerInfo();
        }

        return null;
    }
}
