using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace FOMS.Infrastructure.Security;

public static class SimpleLoginAuthenticationExtensions
{
    public static AuthenticationBuilder AddFomsSimpleLoginAuthentication(this IServiceCollection services)
    {
        return services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(config =>
            {
                config.Cookie.Name = AuthenticationConstants.CookieName;
                config.Cookie.HttpOnly = true;
                config.Cookie.Path = "/";
                config.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                config.Cookie.SameSite = SameSiteMode.None;
            });
    }
}
