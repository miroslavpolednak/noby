using CIS.Infrastructure.StartupExtensions;
using CIS.InternalServices.NotificationService.Api.BackgroundServices.SendEmails;
using CIS.InternalServices.NotificationService.Api.BackgroundServices.SetExpiredEmails;

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
                config?.EmailFormats?.Any() ?? false,
                $"{nameof(AppConfiguration)}.{nameof(AppConfiguration.EmailFormats)} cannot be empty.")
            
            .Validate(config =>
                    config?.EmailLanguageCodes?.Any() ?? false,
                $"{nameof(AppConfiguration)}.{nameof(AppConfiguration.EmailLanguageCodes)} cannot be empty.")
            
            .Validate(config =>
                !string.IsNullOrEmpty(config?.KafkaTopics?.McsResult),
                $"{nameof(AppConfiguration)}.{nameof(AppConfiguration.KafkaTopics)}.{nameof(KafkaTopics.McsResult)} required.")
            .Validate(config =>
                    !string.IsNullOrEmpty(config?.KafkaTopics?.McsSender),
                $"{nameof(AppConfiguration)}.{nameof(AppConfiguration.KafkaTopics)}.{nameof(KafkaTopics.McsSender)} required.")
            
            .Validate(config =>
                !string.IsNullOrEmpty(config?.S3Buckets?.Mcs),
                $"{nameof(AppConfiguration)}.{nameof(AppConfiguration.S3Buckets)}.{nameof(S3Buckets.Mcs)} required.")
            .ValidateOnStart();
        
        string sendEmailsJobConfiguration = $"{CisBackgroundServiceExtensions.ConfigurationSectionKey}:{nameof(SendEmailsJob)}:{CisBackgroundServiceExtensions.CustomConfigurationSectionKey}";
        builder.Services
            .AddOptions<SendEmailsJobConfiguration>()
            .Bind(builder.Configuration.GetSection(sendEmailsJobConfiguration))
            .Validate(
                config => !string.IsNullOrEmpty(config?.SmtpConfiguration.Host),
                $"{sendEmailsJobConfiguration}.{nameof(SmtpConfiguration)}.{nameof(SmtpConfiguration.Host)} required.")
            .Validate(
                config => config?.SmtpConfiguration.Port != 0,
                $"{sendEmailsJobConfiguration}.{nameof(SmtpConfiguration)}.{nameof(SmtpConfiguration.Port)} required and cannot be 0.")
            .Validate(
                config => config?.SmtpConfiguration.Timeout >= 10 && config?.SmtpConfiguration.Timeout <= 300,
                $"{sendEmailsJobConfiguration}.{nameof(SmtpConfiguration)}.{nameof(SmtpConfiguration.Timeout)} is required and value must be between 10 and 300 seconds.")
            .Validate(
                config => config?.NumberOfEmailsAtOnce > 0,
                $"{sendEmailsJobConfiguration}.NumberOfEmailsAtOnce required and cannot be 0.")
            .Validate(config => config?.EmailDomainWhitelist != null,
                $"{sendEmailsJobConfiguration}.EmailDomainWhitelist required.")
            .ValidateOnStart();

        string setExpiredEmailsJobConfiguration = $"{CisBackgroundServiceExtensions.ConfigurationSectionKey}:{nameof(SetExpiredEmailsJob)}:{CisBackgroundServiceExtensions.CustomConfigurationSectionKey}";
        builder.Services
            .AddOptions<SetExpiredEmailsJobConfiguration>()
            .Bind(builder.Configuration.GetSection(setExpiredEmailsJobConfiguration))
            .Validate(
                config => config?.EmailSlaInMinutes > 0,
                $"{setExpiredEmailsJobConfiguration}.EmailSlaInMinutes required and cannot be 0.")
            .ValidateOnStart();

        return builder;
    }
}