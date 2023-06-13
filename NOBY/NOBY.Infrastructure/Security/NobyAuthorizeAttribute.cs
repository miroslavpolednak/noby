using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NOBY.Infrastructure.Security;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public sealed class NobyAuthorizeAttribute 
    : TypeFilterAttribute
{
    public NobyAuthorizeAttribute(int requiredPermission)
        : base(typeof(NobyAuthorizeFilter))
    {
        Arguments = new object[] { requiredPermission };
    }

    private sealed class NobyAuthorizeFilter
        : IAuthorizationFilter
    {
        private readonly int _requiredPermission;

        public NobyAuthorizeFilter(int requiredPermission)
        {
            _requiredPermission = requiredPermission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext!.User.HasClaim(t => t.Type == AuthenticationConstants.NobyPermissionClaimType && t.Value == $"{_requiredPermission}"))
            {
                throw new CisAuthorizationException();
            }
        }
    }
}
