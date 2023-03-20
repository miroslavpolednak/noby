using DomainServices.UserService.Clients;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace NOBY.Infrastructure.Security.CaasAuthentication;

internal sealed class CaasCookieHandler
    : IConfigureNamedOptions<CookieAuthenticationOptions>
{
    public void Configure(string? name, CookieAuthenticationOptions options)
    {
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

                // zavolat user service a zjistit, jestli muze uzivatel do aplikace
                var userServiceClient = (IUserServiceClient)context.HttpContext.RequestServices.GetService(typeof(IUserServiceClient))!;
                var userInstance = await userServiceClient.GetUserByLogin(currentLogin);

                //TODO nejaka kontrola prav?

                // vytvorit claimy
                var claims = new List<Claim>();
                claims.Add(new Claim(CIS.Core.Security.SecurityConstants.ClaimTypeIdent, currentLogin));
                claims.Add(new Claim(CIS.Core.Security.SecurityConstants.ClaimTypeId, userInstance.Id.ToString(System.Globalization.CultureInfo.InvariantCulture)));

                var identity = new ClaimsIdentity(claims, context.Principal.Identity!.AuthenticationType, CIS.Core.Security.SecurityConstants.ClaimTypeId, "role");
                var principal = new ClaimsPrincipal(identity);

                // ulozit nove vytvorenou identitu
                context.Principal = principal;
            },
            OnSignedIn = context =>
            {
                context.Properties.RedirectUri = context.HttpContext.Items["noby_redirect_uri"]!.ToString();
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
