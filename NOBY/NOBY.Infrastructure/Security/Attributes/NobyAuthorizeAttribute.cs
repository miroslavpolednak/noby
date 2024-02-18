using DomainServices.UserService.Clients.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NOBY.Infrastructure.Security.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public sealed class NobyAuthorizeAttribute
    : TypeFilterAttribute
{
    public UserPermissions[] RequiredPermissions { get; init; }
    public bool MustSatisfyAll { get; init; }

    public NobyAuthorizeAttribute(bool mustSatisfyAll, params UserPermissions[] requiredPermissions)
        : base(typeof(NobyAuthorizeFilter))
    {
        MustSatisfyAll = mustSatisfyAll;
        RequiredPermissions = requiredPermissions;
        Arguments = new object[] { MustSatisfyAll, requiredPermissions };
    }

    public NobyAuthorizeAttribute(params UserPermissions[] requiredPermissions)
        : base(typeof(NobyAuthorizeFilter))
    {
        MustSatisfyAll = true;
        RequiredPermissions = requiredPermissions;
        Arguments = new object[] { MustSatisfyAll, requiredPermissions };
    }

    private sealed class NobyAuthorizeFilter
        : IAuthorizationFilter
    {
        private readonly UserPermissions[] _requiredPermissions;
        private readonly bool _mustSatisfyAll;

        public NobyAuthorizeFilter(bool mustSatisfyAll, UserPermissions[] requiredPermissions)
        {
            _mustSatisfyAll = mustSatisfyAll;
            _requiredPermissions = requiredPermissions;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string[] perms = _requiredPermissions.Select(t => $"{(int)t}").ToArray();

            if (_mustSatisfyAll)
            {
                if (!perms.All(t => context
                    .HttpContext!
                    .User
                    .Claims
                    .Any(x => x.Type == AuthenticationConstants.NobyPermissionClaimType && x.Value == t)))
                {
                    throw new CisAuthorizationException($"User does not have all of claims {string.Join(",", perms)}");
                }
            }
            else
            {
                if (!perms.Any(t => context
                    .HttpContext!
                    .User
                    .Claims
                    .Any(x => x.Type == AuthenticationConstants.NobyPermissionClaimType && x.Value == t)))
                {
                    throw new CisAuthorizationException($"User does not have all of claims {string.Join(",", perms)}");
                }
            }
        }
    }
}
