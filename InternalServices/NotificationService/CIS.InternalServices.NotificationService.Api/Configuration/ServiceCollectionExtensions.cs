using Microsoft.Extensions.Options;

namespace CIS.InternalServices.NotificationService.Api.Configuration;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddOptions<AppConfiguration>()
            .Bind(builder.Configuration.GetSection(nameof(AppConfiguration)))
            .Validate(config =>
                config != null,
                $"{nameof(AppConfiguration)} required.")
            .ValidateOnStart();
        
        builder.Services
            .AddOptions<KafkaConfiguration>()
            .Bind(builder.Configuration.GetSection(nameof(KafkaConfiguration)))
            .Validate(config => !string.IsNullOrEmpty(config.ConnectionStrings?.Application),
                $"{nameof(KafkaConfiguration)}.{nameof(KafkaConfiguration.ConnectionStrings)}.{nameof(KafkaConfiguration.ConnectionStrings.Application)} required."
            )
            .Validate(config => !string.IsNullOrEmpty(config.ConnectionStrings?.Logging),
                $"{nameof(KafkaConfiguration)}.{nameof(KafkaConfiguration.ConnectionStrings)}.{nameof(KafkaConfiguration.ConnectionStrings.Logging)} required."
            )
            .ValidateOnStart();
        
        return builder;
    }

    public static KafkaConfiguration GetKafkaConfiguration(this WebApplicationBuilder builder)
    {
        var provider = builder.Services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<KafkaConfiguration>>();
        return options.Value;
    }
}