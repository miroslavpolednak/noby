using CIS.Infrastructure.Messaging.Configuration;
using KafkaFlow.Configuration;
using KafkaFlow.Retry;
using KafkaFlow.Retry.SqlServer;
using Microsoft.Data.SqlClient;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Configuration.RetryStrategy;

internal sealed class DurableKafkaRetryStrategy : IKafkaRetryStrategy
{
    private readonly KafkaFlowConfiguration _kafkaConfiguration;
    private readonly string _connectionString;

    public DurableKafkaRetryStrategy(KafkaFlowConfiguration kafkaConfiguration, string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString, nameof(connectionString));

        _kafkaConfiguration = kafkaConfiguration;
        _connectionString = connectionString;
    }

    public void Configure(IConsumerMiddlewareConfigurationBuilder middlewaresBuilder)
    {
        var connectionStringBuilder = new SqlConnectionStringBuilder(_connectionString);

        middlewaresBuilder.RetryDurable(config =>
        {
            config.HandleAnyException()
                  .WithMessageType(typeof(object)) //mandatory, but its only used for embedded retry topic.
                  .WithEmbeddedRetryCluster(default, retryClusterConfig => retryClusterConfig.Enabled(false))
                  .WithPollingJobsConfiguration(pollingConfig =>
                  {
                      pollingConfig.WithSchedulerId("kafka_durable_retry")
                                   .WithRetryDurablePollingConfiguration(durablePolling =>
                                   {
                                       durablePolling.WithFetchSize(10)
                                                     .WithCronExpression("0 0/1 * 1/1 * ? *")
                                                     .WithExpirationIntervalFactor(1)
                                                     .Enabled(true);
                                   });
                  }).WithSqlServerDataProvider(_connectionString, connectionStringBuilder.DataSource, (string)"msg")
                  .WithRetryPlanBeforeRetryDurable(retryConfig =>
                  {
                      retryConfig.TryTimes(_kafkaConfiguration.RetryTimes)
                                 .WithTimeBetweenTriesPlan(retryCount => TimeSpan.FromMilliseconds(Math.Pow(2, retryCount) * _kafkaConfiguration.TimeBetweenTriesMs))
                                 .ShouldPauseConsumer(false);
                  });
        });
    }
}