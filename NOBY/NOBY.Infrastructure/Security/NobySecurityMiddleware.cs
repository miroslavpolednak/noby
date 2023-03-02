using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace NOBY.Infrastructure.Security;

public class AppSecurityMiddleware
{
    private readonly RequestDelegate _next;

    public AppSecurityMiddleware(RequestDelegate next) =>
        _next = next;

    public async Task Invoke(
        HttpContext context, 
        DomainServices.UserService.Clients.IUserServiceClient userService, 
        Configuration.AppConfiguration configuration)
    {
        bool authenticateUser = true;

        if (configuration.Security!.AuthenticationScheme != AuthenticationConstants.CaasAuthScheme)
        {
            //TODO v net5 nefunguje context.GetEndpoint(). Jak tohle vyresit lepe?
            authenticateUser = !_anonymousUrl.Contains(context.Request.Path.ToString());
        }

        if (authenticateUser)
        {
            if (context.User?.Identity is null || !context.User.Identity.IsAuthenticated)
                throw new System.Security.Authentication.AuthenticationException("User Identity not found in HttpContext");

            // zjistit login uzivatele
            var login = (context.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(t => t.Type == ClaimTypes.Spn)?.Value;
            if (string.IsNullOrEmpty(login))
                throw new System.Security.Authentication.AuthenticationException("User login is empty");

            // ziskat instanci uzivatele z xxv
            var result = await userService.GetUserByLogin(login);

            // vlozit FOMS uzivatele do contextu
            context.User = new NobyUser(context.User.Identity, result);
        }

        await _next.Invoke(context);
    }

    private static string[] _anonymousUrl = new[]
    {
        "/api/users/signin",
        "/api/admin/discovery-service"
    };
}
