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
                config?.Consumers != null,
                $"{nameof(AppConfiguration)}.{nameof(AppConfiguration.Consumers)} required.")
            .Validate(config =>
                config?.Consumers?.TrueForAll(c => !string.IsNullOrEmpty(c.Username) && !string.IsNullOrEmpty(c.ConsumerId)) ?? false,
                $"{nameof(AppConfiguration)}.{nameof(AppConfiguration.Consumers)} have {nameof(Consumer.Username)} and {nameof(Consumer.ConsumerId)} required.")
            .Validate(config =>
                config?.Consumers?.Select(c => c.Username).Distinct().Count() == config?.Consumers?.Count,
                $"{nameof(AppConfiguration)}.{nameof(AppConfiguration.Consumers)} must contain {nameof(Consumer)} with unique {nameof(Consumer.Username)}.")

            .Validate(config =>
                    config?.EmailSenders?.Mcs?.Any() ?? false,
                $"{nameof(AppConfiguration)}.{nameof(AppConfiguration.EmailSenders)}.{nameof(EmailSenders.Mcs)} cannot be empty.")
            .Validate(config =>
                    config?.EmailSenders?.Mpss?.Any() ?? false,
                $"{nameof(AppConfiguration)}.{nameof(AppConfiguration.EmailSenders)}.{nameof(EmailSenders.Mpss)} cannot be empty.")
            
            .Validate(config =>
                !string.IsNullOrEmpty(config?.KafkaTopics?.McsResult),
                $"{nameof(AppConfiguration)}.{nameof(AppConfiguration.KafkaTopics)}.{nameof(KafkaTopics.McsResult)} required.")
            .Validate(config =>
                    !string.IsNullOrEmpty(config?.KafkaTopics?.McsSender),
                $"{nameof(AppConfiguration)}.{nameof(AppConfiguration.KafkaTopics)}.{nameof(KafkaTopics.McsSender)} required.")
            .Validate(config =>
                    !string.IsNullOrEmpty(config?.KafkaTopics?.NobySendEmail),
                $"{nameof(AppConfiguration)}.{nameof(AppConfiguration.KafkaTopics)}.{nameof(KafkaTopics.NobySendEmail)} required.")
            
            .Validate(config =>
                !string.IsNullOrEmpty(config?.S3Buckets?.Mcs),
                $"{nameof(AppConfiguration)}.{nameof(AppConfiguration.S3Buckets)}.{nameof(S3Buckets.Mcs)} required.")
            .Validate(config =>
                    !string.IsNullOrEmpty(config?.S3Buckets?.Mpss),
                $"{nameof(AppConfiguration)}.{nameof(AppConfiguration.S3Buckets)}.{nameof(S3Buckets.Mpss)} required.")
            .ValidateOnStart();
        
        builder.Services
            .AddOptions<S3Configuration>()
            .Bind(builder.Configuration.GetSection(nameof(S3Configuration)))
            .Validate(config => !string.IsNullOrEmpty(config?.ServiceUrl),
                $"{nameof(S3Configuration)}.{nameof(S3Configuration.ServiceUrl)} required.")
            .Validate(config => !string.IsNullOrEmpty(config?.AccessKey),
                $"{nameof(S3Configuration)}.{nameof(S3Configuration.AccessKey)} required.")
            .Validate(config => !string.IsNullOrEmpty(config?.SecretKey),
                $"{nameof(S3Configuration)}.{nameof(S3Configuration.SecretKey)} required.");

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

        builder.Services
            .AddOptions<SmtpConfiguration>()
            .Bind(builder.Configuration.GetSection(nameof(SmtpConfiguration)))
            .Validate(
                config => !string.IsNullOrEmpty(config?.Host),
                $"{nameof(SmtpConfiguration)}.{nameof(SmtpConfiguration.Host)} required.")
            .Validate(
                config => config?.Port != 0,
                $"{nameof(SmtpConfiguration)}.{nameof(SmtpConfiguration.Port)} required and cannot be 0.")
            .ValidateOnStart();
        
        return builder;
    }

    public static AppConfiguration GetAppConfiguration(this WebApplicationBuilder builder)
    {
        return builder.GetConfiguration<AppConfiguration>();
    }
    
    public static S3Configuration GetS3Configuration(this WebApplicationBuilder builder)
    {
        return builder.GetConfiguration<S3Configuration>();
    }
    
    public static KafkaConfiguration GetKafkaConfiguration(this WebApplicationBuilder builder)
    {
        return builder.GetConfiguration<KafkaConfiguration>();
    }

    public static SmtpConfiguration GetSmtpConfiguration(this WebApplicationBuilder builder)
    {
        return builder.GetConfiguration<SmtpConfiguration>();
    }
    
    private static TConfiguration GetConfiguration<TConfiguration>(this WebApplicationBuilder builder)
        where TConfiguration : class
    {
        var provider = builder.Services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<TConfiguration>>();
        return options.Value;
    }
}