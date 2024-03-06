using CIS.Infrastructure.Messaging.Configuration;
using KafkaFlow.Configuration;
using KafkaFlow.Retry;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Configuration.RetryStrategy;

internal sealed class DefaultKafkaRetryStrategy : IKafkaRetryStrategy
{
    private readonly KafkaFlowConfiguration _kafkaConfiguration;

    public DefaultKafkaRetryStrategy(KafkaFlowConfiguration kafkaConfiguration)
    {
        _kafkaConfiguration = kafkaConfiguration;
    }

    public void Configure(IConsumerMiddlewareConfigurationBuilder middlewaresBuilder)
    {
        middlewaresBuilder.RetrySimple(configure =>
        {
            configure.HandleAnyException()
                     .TryTimes(_kafkaConfiguration.RetryTimes)
                     .WithTimeBetweenTriesPlan(retryCount => TimeSpan.FromMilliseconds(Math.Pow(2, retryCount) * _kafkaConfiguration.TimeBetweenTriesMs))
                     .ShouldPauseConsumer(false);
        });
    }
}