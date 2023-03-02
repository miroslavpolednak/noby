using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NOBY.Infrastructure.Security.CaasAuthentication;

namespace NOBY.Infrastructure.Security;

public static class CaasAuthenticationExtensions
{
    public static AuthenticationBuilder AddFomsCaasAuthentication(this IServiceCollection services)
    {
        // persistance refresh tokenu
        services.AddOpenIdConnectAccessTokenManagement();

        // zpusob vytvareni autentizacni cookie
        services.AddSingleton<IConfigureOptions<OpenIdConnectOptions>, CaasOpendIdHandler>();

        // nastaveni volani CAASu
        services.AddSingleton<IConfigureOptions<CookieAuthenticationOptions>, CaasCookieHandler>();

        return services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                
            })
            .AddOpenIdConnect();
    }
}
