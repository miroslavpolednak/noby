﻿using CIS.Infrastructure.Security.ContextUser;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DomainServices;
using CIS.Infrastructure.Security.LoginValidator;

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
        var configuration = builder.Configuration
            .GetSection("CisSecurity:ServiceAuthentication")
            .Get<Configuration.CisServiceAuthenticationConfiguration>();

        if (configuration == null || (string.IsNullOrEmpty(configuration.AdHost) && configuration.Validator == Configuration.CisServiceAuthenticationConfiguration.LoginValidators.ActiveDirectory))
            throw new Core.Exceptions.CisConfigurationNotFound("CisSecurity:ServiceAuthentication");

        builder.Services.AddSingleton(configuration);
        // header parser
        builder.Services.TryAddSingleton<IAuthHeaderParser, AuthHeaderParser>();

        // login validator
        switch (configuration.Validator)
        {
            case Configuration.CisServiceAuthenticationConfiguration.LoginValidators.StaticCollection:
                builder.Services.TryAddSingleton<ILoginValidator, StaticLoginValidator>();
                break;
            case Configuration.CisServiceAuthenticationConfiguration.LoginValidators.ActiveDirectory:
                builder.Services.TryAddSingleton<ILoginValidator, AdLoginValidator>();
                break;
            case Configuration.CisServiceAuthenticationConfiguration.LoginValidators.NativeActiveDirectory:
                builder.Services.TryAddSingleton<ILoginValidator, NativeAdLoginValidator>();
                break;
            default:
                throw new System.Security.Authentication.AuthenticationException($"Unknown LoginValidator {configuration.Validator}");
        }

        builder.Services
            .AddAuthentication(InternalServicesAuthentication.DefaultSchemeName)
            .AddScheme<CisServiceAuthenticationOptions, CisServiceAuthenticationHandler>(InternalServicesAuthentication.DefaultSchemeName, options =>
            {
                options.Domain = configuration.Domain;
                options.AdHost = configuration.AdHost;
                options.AdPort = configuration.AdPort ?? 0;
                options.IsSsl = configuration.IsSsl;
            });

        builder.Services.AddAuthorization();

        // helper pro ziskani instance technickeho uzivatele
        builder.Services.AddScoped<Core.Security.IServiceUserAccessor, ServiceUser.CisServiceUserAccessor>();

        // helper pro ziskani aktualniho uzivatele
        builder.Services.AddScoped<Core.Security.ICurrentUserAccessor, CisCurrentContextUserAccessor>();

        //builder.Services.AddUserService(); // ja myslim, ze je to zde z historickych duvodu a neni to treba

        return builder;
    }
}
