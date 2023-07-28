using CIS.Infrastructure.Audit;
using DomainServices.UserService.Clients;
using DomainServices.UserService.Clients.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace NOBY.Infrastructure.Security.CaasAuthentication;

internal sealed class CaasCookieHandler
    : IConfigureNamedOptions<CookieAuthenticationOptions>
{
    public void Configure(string? name, CookieAuthenticationOptions options)
    {
        if (!string.IsNullOrEmpty(_configuration.AuthenticationScheme))
        {
            options.Cookie.Domain = _configuration.AuthenticationCookieDomain;
        }
        options.Cookie.Path = "/";
        options.Cookie.IsEssential = true;
        //options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.Name = AuthenticationConstants.CookieName;

        // pokud je nastaveno odhlasen pri neaktivite uzivatele
        if (_configuration.SessionInactivityTimeout.GetValueOrDefault() > 0)
        {
            options.SlidingExpiration = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(_configuration.SessionInactivityTimeout!.Value);
        }

        options.Events = new CookieAuthenticationEvents
        {
            OnSigningIn = async context =>
            {
                // login, ktery prisel z CAASu
                var currentLogin = context.Principal!.Claims.First(t => t.Type == ClaimTypes.NameIdentifier).Value;

                var userServiceClient = context.HttpContext.RequestServices.GetRequiredService<IUserServiceClient>();

                // zavolat user service a zjistit, jestli muze uzivatel do aplikace
                var userInstance = await userServiceClient.GetUser(currentLogin);
                // ziskat instanci uzivatele z xxv
                var permissions = await userServiceClient.GetUserPermissions(userInstance.UserId);

                // kontrola, zda ma uzivatel pravo na aplikaci jako takovou
                if (!permissions.Contains((int)UserPermissions.APPLICATION_BasicAccess))
                {
                    throw new CisAuthorizationException();
                }

                // vytvorit claimy
                var claims = new List<Claim>();
                claims.Add(new Claim(CIS.Core.Security.SecurityConstants.ClaimTypeIdent, currentLogin));
                claims.Add(new Claim(CIS.Core.Security.SecurityConstants.ClaimTypeId, userInstance.UserId.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                // doplnit prava uzivatele do claims
                claims.AddRange(permissions.Select(t => new Claim(AuthenticationConstants.NobyPermissionClaimType, $"{t}")));

                var identity = new ClaimsIdentity(claims, context.Principal.Identity!.AuthenticationType, CIS.Core.Security.SecurityConstants.ClaimTypeId, "role");
                var principal = new ClaimsPrincipal(identity);

                // ulozit nove vytvorenou identitu
                context.Principal = principal;

                // zalogovat prihlaseni uzivatele
                var logger = context.HttpContext.RequestServices.GetRequiredService<IAuditLogger>();
                logger.Log(
                    CIS.Infrastructure.Audit.AuditEventTypes.Noby002,
                    $"Uživatel {currentLogin} se přihlásil do aplikace.",
                    bodyAfter: new Dictionary<string, string>() { { "login", currentLogin } });
            },
            OnSignedIn = context =>
            {
                context.Properties.RedirectUri = context.HttpContext.Items["noby_redirect_uri"]!.ToString();
                return Task.CompletedTask;
            },
            OnSigningOut = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<IAuditLogger>();
                logger.Log(CIS.Infrastructure.Audit.AuditEventTypes.Noby003, "User logged out xxxxx");
                return Task.CompletedTask;
            }
        };
    }

    public void Configure(CookieAuthenticationOptions options)
    {
        Configure(Options.DefaultName, options);
    }

    private readonly Configuration.AppConfigurationSecurity _configuration;

    public CaasCookieHandler(Configuration.AppConfiguration configuration)
    {
        _configuration = configuration.Security!;
    }
}
