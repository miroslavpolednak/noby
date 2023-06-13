﻿using Microsoft.AspNetCore.Http;
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

            var claimsIdentity = context.User.Identity as ClaimsIdentity;

            // zjistit login uzivatele
            var userIdClaimValue = claimsIdentity?
                .Claims
                .FirstOrDefault(t => t.Type == CIS.Core.Security.SecurityConstants.ClaimTypeId)?
                .Value;

            if (!int.TryParse(userIdClaimValue, out int userId))
                throw new System.Security.Authentication.AuthenticationException("User login is empty");

            // ziskat instanci uzivatele z xxv
            var result = await userService.GetUser(userId);

            // kontrola, zda ma uzivatel pravo na aplikaci jako takovou

            // doplnit prava uzivatele do claims
            claimsIdentity!.AddClaims(result.UserPermissions.Select(t => new Claim(AuthenticationConstants.NobyPermissionClaimType, $"{t}")));

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
