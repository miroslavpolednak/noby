using CIS.Core.Configuration;
using CIS.Infrastructure.Messaging.Configuration;
using CIS.Infrastructure.Messaging.KafkaFlow.Configuration.RetryStrategy;
using Microsoft.Extensions.Configuration;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Configuration;

internal sealed class KafkaFlowConfiguratorSettings
{
    public KafkaFlowConfiguratorSettings(IConfiguration configuration)
    {
        GroupId = GetGroupId(configuration);
        Configuration = configuration.GetSection(GetKafkaConfigurationPath()).Get<KafkaFlowConfiguration>()!;
        RetryStrategy = GetKafkaRetryStrategy(configuration, Configuration);
    }

    public string GroupId { get; }

    public KafkaFlowConfiguration Configuration { get; }

    public IKafkaRetryStrategy RetryStrategy { get; }

    private static string GetGroupId(IConfiguration configuration)
    {
        var environmentConfiguration = configuration
                                       .GetSection(Core.CisGlobalConstants.EnvironmentConfigurationSectionName)
                                       .Get<CisEnvironmentConfiguration>()!;

        return $"NOBY.{environmentConfiguration.DefaultApplicationKey}-{environmentConfiguration.EnvironmentName}";
    }

    private static string GetKafkaConfigurationPath() =>
        $"{Constants.MessagingConfigurationElement}{ConfigurationPath.KeyDelimiter}{Constants.KafkaConfigurationElement}";

    private static IKafkaRetryStrategy GetKafkaRetryStrategy(IConfiguration configuration, KafkaFlowConfiguration kafkaConfiguration)
    {
        return kafkaConfiguration.RetryPolicy switch
        {
            RetryPolicy.Durable => new DurableKafkaRetryStrategy(kafkaConfiguration, configuration.GetConnectionString("default")!),
            _ => new DefaultKafkaRetryStrategy(kafkaConfiguration)
        };
    }
}