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
        options.SlidingExpiration = true;
        options.Cookie.Name = AuthenticationConstants.CookieName;

        // pokud je nastaveno odhlasen pri neaktivite uzivatele
        if (_configuration.SessionInactivityTimeout.GetValueOrDefault() > 0)
        {
            options.ExpireTimeSpan = TimeSpan.FromMinutes(_configuration.SessionInactivityTimeout!.Value);
        }

        options.Events.OnSigningIn = context =>
        {
            var currentLogin = context.Principal!.Claims.First(t => t.Type == ClaimTypes.NameIdentifier).Value;

            var claims = new List<Claim>();
            claims.Add(new Claim(AuthenticationConstants.ClaimNameLogin, currentLogin));

            var identity = new ClaimsIdentity(claims, context.Principal.Identity!.AuthenticationType, AuthenticationConstants.ClaimNameLogin, "role");
            var principal = new ClaimsPrincipal(identity);

            context.Principal = principal;

            return Task.CompletedTask;
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
