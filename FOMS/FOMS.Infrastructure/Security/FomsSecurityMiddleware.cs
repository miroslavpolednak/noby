using CIS.Core.Results;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FOMS.Infrastructure.Security;

public class AppSecurityMiddleware
{
    private readonly RequestDelegate _next;

    public AppSecurityMiddleware(RequestDelegate next) =>
        _next = next;

    public async Task Invoke(HttpContext context, DomainServices.UserService.Abstraction.IUserServiceAbstraction userService)
    {
        if (context.User?.Identity is null || !context.User.Identity.IsAuthenticated)
            throw new System.Security.Authentication.AuthenticationException("User Identity not found in HttpContext");

        // zjistit login uzivatele
        var login = (context.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(login))
            throw new System.Security.Authentication.AuthenticationException("User login is empty");

        // ziskat instanci uzivatele z xxv
        var result = resolveResult(await userService.GetUserByLogin(login));

        // vlozit FOMS uzivatele do contextu
        context.User = new FomsUser(context.User.Identity, result);

        await _next.Invoke(context);
    }

    private DomainServices.UserService.Contracts.User resolveResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<DomainServices.UserService.Contracts.User> r => r.Model,
            ErrorServiceCallResult err => throw new System.Security.Authentication.AuthenticationException(err.ToString()),
            _ => throw new NotImplementedException()
        };
}
