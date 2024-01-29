using CIS.Infrastructure.BackgroundServices;

namespace CIS.Infrastructure.StartupExtensions;

public static class CisBackgroundServiceExtensions
{
    /// <summary>
    /// Registruje background service job pro periodicke spousteni pomoci crontabu. Zaroven registruje do DI instanci custom konfigurace jobu nahranou z prislusneho objektu v appsettings.json.
    /// </summary>
    /// <typeparam name="TBackgroundService">Typ background service jobu, ktery konfiguraci vyzaduje</typeparam>
    /// <typeparam name="TConfiguration">Typ konfigurace</typeparam>
    /// <param name="validateConfiguration">Validacni funkce, ktera se zavola po nacteni konfigurace. V pripade chybne konfigurace by se uvnitr akce mela vyhazovat vyjimka CisConfigurationException.</param>
    /// <exception cref="Core.Exceptions.CisConfigurationException">Vyjimku vraci funkce validateConfiguration pokud neni spravne nastavena konfigurace jobu.</exception>
    /// <exception cref="Core.Exceptions.CisConfigurationNotFound"></exception>
    public static WebApplicationBuilder AddCisBackgroundService<TBackgroundService, TConfiguration>(this WebApplicationBuilder builder, Action<TConfiguration>? validateConfiguration = null)
        where TBackgroundService : class, ICisBackgroundServiceJob
        where TConfiguration : class, new()
    {
        addServiceAndWorker<TBackgroundService>(builder);
        addCustomConfiguration<TBackgroundService, TConfiguration>(builder, validateConfiguration);

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

    private static void addCustomConfiguration<TBackgroundService, TConfiguration>(WebApplicationBuilder builder, Action<TConfiguration>? validateConfiguration = null)
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

        // validate configuration if requested
        if (validateConfiguration != null)
        {
            validateConfiguration(configuration);
        }
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
