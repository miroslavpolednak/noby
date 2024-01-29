using CIS.Infrastructure.BackgroundServices;
using FluentValidation;

namespace CIS.Infrastructure.StartupExtensions;

public static class CisBackgroundServiceExtensions
{
    /// <summary>
    /// Registruje background service job pro periodicke spousteni pomoci crontabu. Zaroven registruje do DI instanci custom konfigurace jobu nahranou z prislusneho objektu v appsettings.json.
    /// </summary>
    /// <typeparam name="TBackgroundService">Typ background service jobu, ktery konfiguraci vyzaduje</typeparam>
    /// <typeparam name="TConfiguration">Typ konfigurace</typeparam>
    /// <param name="validateConfiguration">Validator pro kontrolu custom konfigurace (FluentValidation).</param>
    /// <exception cref="Core.Exceptions.CisConfigurationException">Vyjimku vraci funkce validateConfiguration pokud neni spravne nastavena konfigurace jobu.</exception>
    /// <exception cref="Core.Exceptions.CisConfigurationNotFound"></exception>
    public static WebApplicationBuilder AddCisBackgroundService<TBackgroundService, TConfiguration>(this WebApplicationBuilder builder, AbstractValidator<TConfiguration>? validator = null)
        where TBackgroundService : class, ICisBackgroundServiceJob
        where TConfiguration : class, new()
    {
        addServiceAndWorker<TBackgroundService>(builder);
        addCustomConfiguration<TBackgroundService, TConfiguration>(builder, validator);

        return builder;
    }

    /// <summary>
    /// Registruje background service job pro periodicke spousteni pomoci crontabu.
    /// </summary>
    /// <exception cref="Core.Exceptions.CisConfigurationNotFound"></exception>
    public static WebApplicationBuilder AddCisBackgroundService<TBackgroundService>(this WebApplicationBuilder builder)
       where TBackgroundService : class, ICisBackgroundServiceJob
    {
        addServiceAndWorker<TBackgroundService>(builder);

        return builder;
    }

    private static void addCustomConfiguration<TBackgroundService, TConfiguration>(WebApplicationBuilder builder, AbstractValidator<TConfiguration>? validator = null)
        where TBackgroundService : class, ICisBackgroundServiceJob
        where TConfiguration : class, new()
    {
        builder.Services.AddSingleton(services =>
        {
            // nacist konfiguraci sluzby
            string sectionName = $"{ConfigurationSectionKey}:{typeof(TBackgroundService).Name}:{CustomConfigurationSectionKey}";

            var configBuilder = services.GetRequiredService<IConfiguration>();
            var configuration = configBuilder
                .GetSection(sectionName)
                .Get<TConfiguration>()
                ?? throw new Core.Exceptions.CisConfigurationNotFound(sectionName);

            // validate configuration if requested
            if (validator != null)
            {
                var validationResult = validator.Validate(configuration);
                if (!validationResult.IsValid)
                {
                    throw new CIS.Core.Exceptions.CisConfigurationException(0, string.Join("; ", validationResult.Errors.Select(t => t.ErrorMessage)));
                }
            }

            return configuration;
        });
    }

    private static void addServiceAndWorker<TBackgroundService>(WebApplicationBuilder builder)
       where TBackgroundService : class, ICisBackgroundServiceJob
    {
        // nacist konfiguraci sluzby
        string sectionName = $"{ConfigurationSectionKey}:{typeof(TBackgroundService).Name}";

        var configuration = builder.Configuration
                .GetSection(sectionName)
                .Get<CisBackgroundServiceConfiguration<TBackgroundService>>()
            ?? throw new Core.Exceptions.CisConfigurationNotFound(sectionName);

        // ulozit konfiguraci sluzby do DI
        builder.Services.AddSingleton<ICisBackgroundServiceConfiguration<TBackgroundService>>(configuration);

        // pridat worker
        builder.Services.AddScoped<TBackgroundService>();
        // pridat ihostedservice
        builder.Services.AddHostedService<CisBackgroundService<TBackgroundService>>();
    }

    /// <summary>
    /// Klic v rootu appsettings.json, kde jsou konfigurovany vsechny background services.
    /// </summary>
    public const string ConfigurationSectionKey = "BackgroundServices";

    /// <summary>
    /// Klic v ramci konfigurace kazde background service, pod kterym muze byt ulozena custom konfigurace jobu.
    /// </summary>
    public const string CustomConfigurationSectionKey = "CustomConfiguration";
}
