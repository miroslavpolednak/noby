using CIS.Infrastructure.Security.ContextUser;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CIS.Infrastructure.Security.LoginValidator;
using Microsoft.Extensions.Options;
using System.Security.Authentication;

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
        // add configuration
        builder
            .Services
            .AddOptions<Configuration.CisServiceAuthenticationConfiguration>()
            .Bind(builder.Configuration.GetSection(Core.CisGlobalConstants.ServiceAuthenticationSectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        // header parser
        builder.Services.TryAddSingleton<IAuthHeaderParser, AuthHeaderParser>();
        // add di validator
        builder.Services.AddSingleton<NativeAdLoginValidator>();

        // login validator
        builder.Services.TryAddSingleton<ILoginValidator>(provider =>
        {
            var configuration = provider
                .GetRequiredService<IOptions<Configuration.CisServiceAuthenticationConfiguration>>()
                .Value;

            return configuration.Validator switch
            {
                Configuration.CisServiceAuthenticationConfiguration.LoginValidators.StaticCollection => new StaticLoginValidator(),

                Configuration.CisServiceAuthenticationConfiguration.LoginValidators.NativeActiveDirectory => provider.GetRequiredService<NativeAdLoginValidator>(),
                    
                _ => throw new AuthenticationException($"Unknown LoginValidator {configuration.Validator}")
            };
        });

        // native auth/authorization
        builder.Services
            .AddAuthentication(InternalServicesAuthentication.DefaultSchemeName)
            .AddScheme<CisServiceAuthenticationOptions, CisServiceAuthenticationHandler>(InternalServicesAuthentication.DefaultSchemeName, options => { });
        builder.Services.AddAuthorization();

        // helper pro ziskani instance technickeho uzivatele
        builder.Services.AddScoped<Core.Security.IServiceUserAccessor, ServiceUser.CisServiceUserAccessor>();

        return builder;
    }

    /// <summary>
    /// helper pro ziskani aktualniho uzivatele
    /// </summary>
    public static WebApplicationBuilder AddCisServiceUserContext(this WebApplicationBuilder builder)
    {
        // helper pro ziskani aktualniho uzivatele
        builder.Services.AddScoped<Core.Security.ICurrentUserAccessor, CisCurrentContextUserAccessor>();
        builder.Services.AddTransient<CisCurrentUserAccessorCache>();

        return builder;
    }
}
