using KafkaFlow.Configuration;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Configuration.RetryStrategy;

internal sealed class NoneRetryStrategy : IKafkaRetryStrategy
{
    public void Configure(IConsumerMiddlewareConfigurationBuilder middlewaresBuilder)
    {
    }
}