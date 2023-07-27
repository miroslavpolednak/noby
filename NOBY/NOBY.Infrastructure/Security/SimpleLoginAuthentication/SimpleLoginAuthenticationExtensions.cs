using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NOBY.Infrastructure.Configuration;

namespace NOBY.Infrastructure.Security;

public static class SimpleLoginAuthenticationExtensions
{
    public static AuthenticationBuilder AddFomsSimpleLoginAuthentication(this IServiceCollection services, AppConfigurationSecurity configuration)
    {
        return services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(config =>
            {
                if (!string.IsNullOrEmpty(configuration.AuthenticationScheme))
                {
                    config.Cookie.Domain = configuration.AuthenticationCookieDomain;
                }
                config.Cookie.Name = AuthenticationConstants.CookieName;
                config.Cookie.HttpOnly = true;
                config.Cookie.Path = "/";
                config.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                config.Cookie.SameSite = SameSiteMode.None;
            });
    }
}
