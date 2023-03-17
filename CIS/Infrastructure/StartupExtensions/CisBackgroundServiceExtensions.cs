using CIS.Infrastructure.BackgroundServices;

namespace CIS.Infrastructure.StartupExtensions;

public static class CisBackgroundServiceExtensions
{
    /// <summary>
    /// Zaregistruje do DI instanci custom konfigurace jobu nahranou z prislusneho objektu v appsettings.json
    /// </summary>
    /// <typeparam name="TBackgroundService">Typ background service jobu, ktery konfiguraci vyzaduje</typeparam>
    /// <typeparam name="TConfiguration">Typ konfigurace</typeparam>
    /// <exception cref="Core.Exceptions.CisConfigurationNotFound"></exception>
    public static WebApplicationBuilder AddCisBackgroundServiceCustomConfiguration<TBackgroundService, TConfiguration>(this WebApplicationBuilder builder)
        where TBackgroundService : class, ICisBackgroundServiceJob
        where TConfiguration : class, new()
    {
        // nacist konfiguraci sluzby
        string sectionName = $"{ConfigurationSectionKey}:{typeof(TBackgroundService).Name}:{CustomConfigurationSectionKey}";

        var configuration = builder.Configuration
            .GetSection(sectionName)
            .Get<TConfiguration>()
            ?? throw new Core.Exceptions.CisConfigurationNotFound(sectionName);

        builder.Services.AddSingleton(configuration);

        return builder;
    }

    /// <summary>
    /// Registruje background service job pro periodicke spousteni pomoci crontabu.
    /// </summary>
    /// <exception cref="Core.Exceptions.CisConfigurationNotFound"></exception>
    public static WebApplicationBuilder AddCisBackgroundService<TBackgroundService>(this WebApplicationBuilder builder)
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

        return builder;
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
