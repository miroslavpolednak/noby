﻿using CIS.Core.Security;
using SharedAudit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NOBY.Infrastructure.Configuration;

namespace NOBY.Infrastructure.Security.Endpoints;

public static class MapAuthenticationEndpoints
{
    public static IApplicationBuilder MapNobyAuthenticationEndpoints(this IApplicationBuilder appBuilder)
    {
        return appBuilder.UseEndpoints(t =>
        {
            var aut = t.NewVersionedApi();
            // sign in
            aut.MapGet(AuthenticationConstants.DefaultAuthenticationUrlPrefix + AuthenticationConstants.DefaultSignInEndpoint, () =>
            {
            })
                .RequireAuthorization()
                .Produces(302)
                .WithDescription("Přihlášení uživatele / redirect na auth provider.")
                .WithTags("Users")
                .HasApiVersion(1.0)
                .WithName("loginUserGet")
                .WithOpenApi(generatedOperation =>
                {
                    generatedOperation.Summary = "Přihlášení uživatele / redirect na auth provider.";
                    generatedOperation.Responses.First().Value.Description = "Redirect na CAAS";
                    return generatedOperation;
                });

            // Odhlášení přihlášeného uživatele
            aut.MapGet(AuthenticationConstants.DefaultAuthenticationUrlPrefix + AuthenticationConstants.DefaultSignOutEndpoint,
                ([FromServices] IHttpContextAccessor context,
                [FromServices] AppConfiguration configuration,
                [FromServices] IAuditLogger logger,
                [FromQuery] string? redirect) =>
                {
                    string redirectUrl = Uri.TryCreate(redirect, UriKind.Absolute, out var uri) ? uri.ToString() : "/";
                    string userLogin = context.HttpContext!.User.Claims.First(t => t.Type == SecurityConstants.ClaimTypeIdent).Value;

                    context.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    if (configuration.Security!.AuthenticationScheme == AuthenticationConstants.CaasAuthScheme)
                    {
                        context.HttpContext!.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
                    }

                    logger.Log(AuditEventTypes.Noby003, $"Uživatel {userLogin} se odhlásil z aplikace", bodyBefore: new Dictionary<string, string>
                    {
                        { "Login", userLogin },
                        { "Method", "manually" }
                    });

                    // redirect to root?
                    context.HttpContext!.Response.Redirect(redirectUrl);
                })
                .RequireAuthorization()
                .Produces(302)
                .WithTags("Users")
                .HasApiVersion(1.0)
                .WithName("signoutUserGet")
                .WithOpenApi(generatedOperation =>
                {
                    generatedOperation.Summary = "Odhlášení přihlášeného uživatele";
                    generatedOperation.Description = "Provolání zajistí správné odhlášení přihlášeného uživatele. <br /><br /><a href=\"https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=713A5C7F-13DB-4c88-863B-29C40F2EDE32\"><img src=\"https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png\" width=\"20\" height=\"20\" />Diagram v EA</a>";

                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "Absolutní URI kam má být uživatel přesměrován po odhlášení";

                    generatedOperation.Responses.First().Value.Description = "Uživatel byl odhlášen";

                    return generatedOperation;
                });
        });
    }
}
