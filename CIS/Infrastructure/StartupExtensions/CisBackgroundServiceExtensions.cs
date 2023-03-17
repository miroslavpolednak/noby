using CIS.Infrastructure.BackgroundServices;

namespace CIS.Infrastructure.StartupExtensions;

public static class CisBackgroundServiceExtensions
{
    public static WebApplicationBuilder AddCisBackgroundService<TBackgroundService>(this WebApplicationBuilder builder)
       where TBackgroundService : class, ICisBackgroundService
    {
        // nacist konfiguraci sluzby
        string sectionName = $"{_configurationSectionKey}:{typeof(TBackgroundService).Name}";

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

    private const string _configurationSectionKey = "BackgroundServices";
}
