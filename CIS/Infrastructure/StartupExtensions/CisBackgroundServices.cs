using CIS.Infrastructure.BackgroundServiceJob;

namespace CIS.Infrastructure.StartupExtensions;

public static class CisBackgroundServices
{
    public static WebApplicationBuilder AddCisPeriodicJob<TBackgroundService, TBackgroundServiceConfiguration>(this WebApplicationBuilder builder)
       where TBackgroundService : class, IPeriodicBackgroundServiceJob
       where TBackgroundServiceConfiguration : class, IPeriodicJobConfiguration<TBackgroundService>, new()
    {
        builder.Services.AddSingleton(_ =>
        {
            var options = new TBackgroundServiceConfiguration();
            builder.Configuration.GetSection(options.SectionName).Bind(options);
            return options;
        });

        builder.Services.AddSingleton<IPeriodicJobConfiguration<TBackgroundService>>(
              svcProvider => svcProvider.GetRequiredService<TBackgroundServiceConfiguration>());

        builder.Services.AddScoped<TBackgroundService>();
        builder.Services.AddHostedService<PeriodicBackgroundJob<TBackgroundService>>();
        return builder;
    }
}
