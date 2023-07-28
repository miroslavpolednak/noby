using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace NOBY.Infrastructure.Security;

public class NobySecurityMiddleware
{
    private readonly RequestDelegate _next;

    public NobySecurityMiddleware(RequestDelegate next) =>
        _next = next;

    public async Task Invoke(
        HttpContext context, 
        DomainServices.UserService.Clients.IUserServiceClient userService, 
        Configuration.AppConfiguration configuration)
    {
        if (authenticateUser())
        {
            if (context.User?.Identity is null || !context.User.Identity.IsAuthenticated)
                throw new System.Security.Authentication.AuthenticationException("User Identity not found in HttpContext");

            var claimsIdentity = context.User.Identity as ClaimsIdentity;

            // zjistit login uzivatele
            var userIdClaimValue = claimsIdentity?
                .Claims
                .FirstOrDefault(t => t.Type == CIS.Core.Security.SecurityConstants.ClaimTypeId)?
                .Value;

            if (!int.TryParse(userIdClaimValue, out int userId))
                throw new System.Security.Authentication.AuthenticationException("User login is empty");

            // vlozit FOMS uzivatele do contextu
            context.User = new NobyUser(context.User.Identity, userId);
        }

        await _next.Invoke(context);

        bool authenticateUser()
        {
            var endpoint = context.GetEndpoint();
            if (endpoint is null) return true;
            return !endpoint.Metadata.OfType<AllowAnonymousAttribute>().Any();
        }
    }
}
