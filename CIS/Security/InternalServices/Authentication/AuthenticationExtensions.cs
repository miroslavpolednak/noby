using CIS.Security.InternalServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CIS.Security;

public static class AuthenticationExtensions
{
    /// <summary>
    /// Prida autentizaci volajici aplikace do aktualni sluzby - pouziva se pouze pro interni sluzby. Autentizace je technickym uzivatelem.
    /// Konfigurace autentizacniho middleware je v CisSecurity:ServiceAuthentication.
    /// </summary>
    public static WebApplicationBuilder AddCisServiceAuthentication(this WebApplicationBuilder builder)
    {
        // get configuration
        var c = new Configuration.CisServiceAuthenticationConfiguration();
        builder.Configuration.GetSection("CisSecurity:ServiceAuthentication").Bind(c);
        if (c == null || (string.IsNullOrEmpty(c.AdHost) && c.Validator == Configuration.CisServiceAuthenticationConfiguration.LoginValidators.ActiveDirectory))
            throw new System.ArgumentNullException("CisSecurity:ServiceAuthentication not configured in appsettings.json");

        // header parser
        builder.Services.TryAddSingleton<IAuthHeaderParser, AuthHeaderParser>();

        // login validator
        switch (c.Validator)
        {
            case Configuration.CisServiceAuthenticationConfiguration.LoginValidators.StaticCollection:
                builder.Services.TryAddSingleton<ILoginValidator, StaticLoginValidator>();
                break;
            case Configuration.CisServiceAuthenticationConfiguration.LoginValidators.ActiveDirectory:
                builder.Services.TryAddSingleton<ILoginValidator, AdLoginValidator>();
                break;
            default:
                throw new System.Exception($"Unknown LoginValidator {c.Validator}");
        }

        builder.Services
            .AddAuthentication(InternalServicesAuthentication.DefaultSchemeName)
            .AddScheme<CisServiceAuthenticationOptions, CisServiceAuthenticationHandler>(InternalServicesAuthentication.DefaultSchemeName, options =>
            {
                options.DomainUsernamePrefix = c.DomainUsernamePrefix;
                options.AdHost = c.AdHost;
                options.AdPort = c.AdPort ?? 0;
            });

        builder.Services.AddAuthorization();

        return builder;
    }
}
