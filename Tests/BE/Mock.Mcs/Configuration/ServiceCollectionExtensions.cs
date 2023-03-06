using Microsoft.Extensions.Options;

namespace Mock.Mcs.Configuration;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddOptions<AppConfiguration>()
            .Bind(builder.Configuration.GetSection(nameof(AppConfiguration)))

            .Validate(config =>
                !string.IsNullOrEmpty(config?.KafkaTopics?.McsResult),
                $"{nameof(AppConfiguration)}.{nameof(AppConfiguration.KafkaTopics)}.{nameof(KafkaTopics.McsResult)} required.")
            .Validate(config =>
                    !string.IsNullOrEmpty(config?.KafkaTopics?.McsSender),
                $"{nameof(AppConfiguration)}.{nameof(AppConfiguration.KafkaTopics)}.{nameof(KafkaTopics.McsSender)} required.")
            .ValidateOnStart();

        builder.Services
            .AddOptions<KafkaConfiguration>()
            .Bind(builder.Configuration.GetSection(nameof(KafkaConfiguration)))
            .Validate(config => !string.IsNullOrEmpty(config?.GroupId),
                $"{nameof(KafkaConfiguration)}.{nameof(KafkaConfiguration.GroupId)} required.")
            .Validate(config => !string.IsNullOrEmpty(config?.Nodes?.Business?.BootstrapServers),
                $"{nameof(KafkaConfiguration)}.{nameof(KafkaConfiguration.Nodes)}.{nameof(KafkaConfiguration.Nodes.Business)}.{nameof(KafkaConfiguration.Nodes.Business.BootstrapServers)} required.")
            .Validate(config => !string.IsNullOrEmpty(config?.Debug),
                $"{nameof(KafkaConfiguration)}.{nameof(KafkaConfiguration.Debug)}")
            .ValidateOnStart();

        return builder;
    }

    public static AppConfiguration GetAppConfiguration(this WebApplicationBuilder builder)
    {
        return builder.GetConfiguration<AppConfiguration>();
    }

    public static KafkaConfiguration GetKafkaConfiguration(this WebApplicationBuilder builder)
    {
        return builder.GetConfiguration<KafkaConfiguration>();
    }

    private static TConfiguration GetConfiguration<TConfiguration>(this WebApplicationBuilder builder)
        where TConfiguration : class
    {
        var provider = builder.Services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<TConfiguration>>();
        return options.Value;
    }
}