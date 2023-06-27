using CIS.Core.Security;
using System.Security.Claims;

namespace NOBY.Infrastructure.Security;

public static class SecurityExtensions
{
    public static bool HasPermission(this ClaimsPrincipal principal, DomainServices.UserService.Clients.Authorization.UserPermissions permission)
        => principal.HasPermission((int)permission);

    public static bool HasPermission(this ClaimsPrincipal principal, int permission)
        => principal.HasClaim(t => t.Type == AuthenticationConstants.NobyPermissionClaimType && t.Value == $"{permission}");

    public static bool HasPermission(this ICurrentUserAccessor userAccessor, DomainServices.UserService.Clients.Authorization.UserPermissions permission)
        => userAccessor.HasPermission((int)permission);

    public static bool HasPermission(this ICurrentUserAccessor userAccessor, int permission)
        => userAccessor.Claims.Any(t => t.Type == AuthenticationConstants.NobyPermissionClaimType && t.Value == $"{permission}");

    public static void CheckPermission(this ClaimsPrincipal principal, DomainServices.UserService.Clients.Authorization.UserPermissions permission)
    {
        if (!principal.HasPermission(permission))
        {
            throw new CisAuthorizationException();
        }
    }

    public static void CheckPermission(this ClaimsPrincipal principal, int permission)
    {
        if (!principal.HasPermission(permission))
        {
            throw new CisAuthorizationException();
        }
    }

    public static void CheckPermission(this ICurrentUserAccessor userAccessor, DomainServices.UserService.Clients.Authorization.UserPermissions permission)
    {
        if (!userAccessor.HasPermission(permission))
        {
            throw new CisAuthorizationException();
        }
    }

    public static void CheckPermission(this ICurrentUserAccessor userAccessor, int permission)
    {
        if (!userAccessor.HasPermission(permission))
        {
            throw new CisAuthorizationException();
        }
    }
}
