using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NOBY.Infrastructure.Security.CaasAuthentication;

namespace NOBY.Infrastructure.Security;

public static class CaasAuthenticationExtensions
{
    public static AuthenticationBuilder AddFomsCaasAuthentication(this IServiceCollection services, Configuration.AppConfigurationSecurity configuration)
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
                // pokud je nastaveno odhlasen pri neaktivite uzivatele
                if (configuration.SessionInactivityTimeout.GetValueOrDefault() > 0)
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(configuration.SessionInactivityTimeout!.Value);
                }
            })
            .AddOpenIdConnect(options =>
            {
                options.ClientId = configuration.ClientId;
                options.ClientSecret = configuration.ClientSecret;
                options.Authority = configuration.Authority;
            });
    }
}
