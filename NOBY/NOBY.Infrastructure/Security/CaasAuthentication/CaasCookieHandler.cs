using SharedAudit;
using DomainServices.UserService.Clients;
using DomainServices.UserService.Clients.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Security.Claims;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace NOBY.Infrastructure.Security.CaasAuthentication;

internal sealed class CaasCookieHandler
    : IConfigureNamedOptions<CookieAuthenticationOptions>
{
    public void Configure(string? name, CookieAuthenticationOptions options)
    {
        var appVersion = Assembly.GetEntryAssembly()!.GetName().Version!.ToString();

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
            OnRedirectToAccessDenied = context =>
            {
                if (isAjaxRequest(context.Request))
                {
                    context.Response.Headers.Location = $"{_configuration.FailedSignInRedirectPath}?reason=authentication_access_denied";
                    context.Response.StatusCode = 403;
                }
                else
                {
                    context.Response.Redirect($"{_configuration.FailedSignInRedirectPath}?reason=authentication_access_denied");
                }
                return Task.CompletedTask;
            },
            OnSigningIn = async context =>
            {
                // login, ktery prisel z CAASu
                var currentLogin = context.Principal!.Claims.First(t => t.Type == ClaimTypes.NameIdentifier).Value;

                var userServiceClient = context.HttpContext.RequestServices.GetRequiredService<IUserServiceClient>();

                // zavolat user service a zjistit, jestli muze uzivatel do aplikace
                DomainServices.UserService.Contracts.User? userInstance = null;

                try
                {
                    userInstance = await userServiceClient.GetUser(currentLogin);
                }
                catch (Exception ex)
                {
                    createLogger(context.HttpContext).UserNotFound(currentLogin, ex);
                    context.Principal = new ClaimsPrincipal();
                    context.Properties.RedirectUri = $"{_configuration.FailedSignInRedirectPath}?reason=authentication_notfound";
                    return;
                }

                // ziskat prava uzivatele z xxv
                var permissions = await userServiceClient.GetUserPermissions(userInstance!.UserId);

                // kontrola, zda ma uzivatel pravo na aplikaci jako takovou
                if (!permissions.Contains((int)UserPermissions.APPLICATION_BasicAccess))
                {
                    createLogger(context.HttpContext).UserWithoutAccess(currentLogin);
                    throw new CisAuthorizationException("Cookie handler: user does not have APPLICATION_BasicAccess");
                }

                // vytvorit claimy
                var claims = new List<Claim>();
                claims.Add(new Claim(CIS.Core.Security.SecurityConstants.ClaimTypeIdent, currentLogin));
                claims.Add(new Claim(CIS.Core.Security.SecurityConstants.ClaimTypeId, userInstance.UserId.ToString(CultureInfo.InvariantCulture)));
                // doplnit prava uzivatele do claims
                claims.AddRange(permissions.Select(t => new Claim(AuthenticationConstants.NobyPermissionClaimType, $"{t}")));

                var identity = new ClaimsIdentity(claims, context.Principal.Identity!.AuthenticationType, CIS.Core.Security.SecurityConstants.ClaimTypeId, "role");
                var principal = new ClaimsPrincipal(identity);

                // ulozit nove vytvorenou identitu
                context.Principal = principal;

                // zalogovat prihlaseni uzivatele
                var auditLogger = context.HttpContext.RequestServices.GetRequiredService<IAuditLogger>();
                auditLogger.Log(
                    AuditEventTypes.Noby002,
                    $"Uživatel {currentLogin} se přihlásil do aplikace.",
                    bodyAfter: new Dictionary<string, string>()
                    {
                        { "login", currentLogin },
                        { "app_version", appVersion }
                    });
            },
            OnSignedIn = context =>
            {
                context.Properties.RedirectUri = context.HttpContext.Items["noby_redirect_uri"]!.ToString();
                return Task.CompletedTask;
            }
        };
    }

    private static bool isAjaxRequest(HttpRequest request)
    {
        return string.Equals(request.Query[HeaderNames.XRequestedWith], "XMLHttpRequest", StringComparison.Ordinal) ||
            string.Equals(request.Headers.XRequestedWith, "XMLHttpRequest", StringComparison.Ordinal);
    }

    private static ILogger<CaasCookieHandler> createLogger(HttpContext context)
    {
        return context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger<CaasCookieHandler>();
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
