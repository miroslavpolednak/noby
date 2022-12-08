using CIS.Infrastructure.Security.ContextUser;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DomainServices;

namespace CIS.Infrastructure.Security;

/// <summary>
/// Extension metody do startupu aplikace.
/// </summary>
public static class StartupExtensions
{
    /// <summary>
    /// Pridava moznost ziskani instance fyzickeho uzivatele volajiciho sluzbu
    /// </summary>
    public static IApplicationBuilder UseCisServiceUserContext(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CisUserContextMiddleware>();
    }

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
            throw new Core.Exceptions.CisConfigurationNotFound("CisSecurity:ServiceAuthentication");

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
                throw new System.Security.Authentication.AuthenticationException($"Unknown LoginValidator {c.Validator}");
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

        // helper pro ziskani instance technickeho uzivatele
        builder.Services.AddScoped<Core.Security.IServiceUserAccessor, ServiceUser.CisServiceUserAccessor>();

        // helper pro ziskani aktualniho uzivatele
        builder.Services.AddScoped<Core.Security.ICurrentUserAccessor, CisCurrentContextUserAccessor>();

        builder.Services.AddUserService();

        return builder;
    }
}
