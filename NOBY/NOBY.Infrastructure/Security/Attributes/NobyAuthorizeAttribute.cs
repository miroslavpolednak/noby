using DomainServices.UserService.Clients.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NOBY.Infrastructure.Security.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public sealed class NobyAuthorizeAttribute
    : TypeFilterAttribute
{
    public UserPermissions[] RequiredPermissions { get; init; }

    public NobyAuthorizeAttribute(params UserPermissions[] requiredPermissions)
        : base(typeof(NobyAuthorizeFilter))
    {
        RequiredPermissions = requiredPermissions;
        Arguments = new object[] { requiredPermissions };
    }

    private sealed class NobyAuthorizeFilter
        : IAuthorizationFilter
    {
        private readonly UserPermissions[] _requiredPermissions;

        public NobyAuthorizeFilter(UserPermissions[] requiredPermissions)
        {
            _requiredPermissions = requiredPermissions;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string[] perms = _requiredPermissions.Select(t => $"{(int)t}").ToArray();

            if (!perms.All(t => context
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
