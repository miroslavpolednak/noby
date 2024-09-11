using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NOBY.Infrastructure.Configuration;
using System.Security.Claims;

namespace NOBY.Infrastructure.Security.Middleware;

public sealed class NobySecurityMiddleware(RequestDelegate _next)
{
    public async Task Invoke(HttpContext context)
    {
        if (authenticateUser())
        {
            if (context.User?.Identity is null || !context.User.Identity.IsAuthenticated)
            {
                throw new System.Security.Authentication.AuthenticationException("User Identity not found in HttpContext");
            }

            var claimsIdentity = context.User.Identity as ClaimsIdentity;

            // zjistit login uzivatele
            var userIdClaimValue = claimsIdentity?
                .Claims
                .FirstOrDefault(t => t.Type == CIS.Core.Security.SecurityConstants.ClaimTypeId)?
                .Value;

            if (!int.TryParse(userIdClaimValue, out int userId))
            {
                throw new System.Security.Authentication.AuthenticationException("User login is empty");
            }

            // vlozit FOMS uzivatele do contextu
            context.User = new NobyUser(context.User.Identity, userId);

            // vratit sliding expiration session uzivatele
			context.Response.OnStarting(() =>
			{
                var timeout = context.RequestServices.GetRequiredService<AppConfiguration>().Security?.SessionInactivityTimeout;

				if (timeout.HasValue && !context.Request.Headers.ContainsKey(AuthenticationConstants.DoNotRenewAuthenticationTicketHeaderKey))
				{
					context.Response.Headers.Append("inactivity-timeout", DateTime.Now.AddMinutes(timeout.Value).ToString("s", CultureInfo.InvariantCulture));
				}

				return Task.CompletedTask;
			});
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
