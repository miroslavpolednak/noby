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
            .Validate(config => !string.IsNullOrEmpty(config.GroupId),
                $"{nameof(KafkaConfiguration)}.{nameof(KafkaConfiguration.GroupId)}")
            .Validate(config => !string.IsNullOrEmpty(config.SchemaRegistryUrl),
                $"{nameof(KafkaConfiguration)}.{nameof(KafkaConfiguration.SchemaRegistryUrl)}")
            .Validate(config => !string.IsNullOrEmpty(config.BootstrapServers?.Business),
                $"{nameof(KafkaConfiguration)}.{nameof(KafkaConfiguration.BootstrapServers)}.{nameof(KafkaConfiguration.BootstrapServers.Business)} required.")
            .Validate(config => !string.IsNullOrEmpty(config.BootstrapServers?.Logman),
                $"{nameof(KafkaConfiguration)}.{nameof(KafkaConfiguration.BootstrapServers)}.{nameof(KafkaConfiguration.BootstrapServers.Logman)} required.")
            .Validate(config => !string.IsNullOrEmpty(config.SslKeystoreLocation),
                $"{nameof(KafkaConfiguration)}.{nameof(KafkaConfiguration.SslKeystoreLocation)}")
            .Validate(config => !string.IsNullOrEmpty(config.SslKeystorePassword),
                $"{nameof(KafkaConfiguration)}.{nameof(KafkaConfiguration.SslKeystorePassword)}")
            .Validate(config => !string.IsNullOrEmpty(config.SecurityProtocol),
                $"{nameof(KafkaConfiguration)}.{nameof(KafkaConfiguration.SecurityProtocol)}")
            .Validate(config => !string.IsNullOrEmpty(config.SslCaLocation),
                $"{nameof(KafkaConfiguration)}.{nameof(KafkaConfiguration.SslCaLocation)}")
            .Validate(config => !string.IsNullOrEmpty(config.SslCertificateLocation),
                $"{nameof(KafkaConfiguration)}.{nameof(KafkaConfiguration.SslCertificateLocation)}")
            .Validate(config => !string.IsNullOrEmpty(config.Debug),
                $"{nameof(KafkaConfiguration)}.{nameof(KafkaConfiguration.Debug)}")
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