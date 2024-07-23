using KafkaFlow.Configuration;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Configuration.RetryStrategy;

internal interface IKafkaRetryStrategy
{
    void Configure(IConsumerMiddlewareConfigurationBuilder middlewaresBuilder);
}