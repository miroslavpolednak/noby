using KafkaFlow;
using KafkaFlow.Admin.Messages;
using KafkaFlow.Consumers;
using KafkaFlow.Producers;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Configuration;

public class MultiBrokerTelemetryScheduler
{
    private static readonly int _processId = Environment.ProcessId;

    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, Timer> _timers = [];
    private readonly ILogHandler _logHandler;

    public MultiBrokerTelemetryScheduler(IServiceProvider serviceProvider, ILogHandler logHandler)
    {
        _serviceProvider = serviceProvider;
        _logHandler = logHandler;
    }

    public void Start(string telemetryId, string topicName, int topicPartition)
    {
        this.Stop(telemetryId);

        var consumers = _serviceProvider.GetRequiredService<IConsumerAccessor>()
                                        .All
                                        .Where(c => !c.ManagementDisabled)
                                        .ToList();

        var producer = _serviceProvider.GetRequiredService<IProducerAccessor>().GetProducer(telemetryId);

        _timers[telemetryId] = new Timer(
            _ => ProduceTelemetry(topicName, topicPartition, consumers, producer),
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(15));
    }

    public void Stop(string telemetryId)
    {
        if (!_timers.TryGetValue(telemetryId, out var timer))
            return;

        timer.Dispose();
        _timers.Remove(telemetryId);
    }

    private void ProduceTelemetry(
        string topicName,
        int partition,
        IEnumerable<IMessageConsumer> consumers,
        IMessageProducer producer)
    {
        try
        {
            var items = consumers
                .SelectMany(
                    c =>
                    {
                        var consumerLag = c.GetTopicPartitionsLag();

                        return c.Topics.Select(
                            topic => new ConsumerTelemetryMetric
                            {
                                ConsumerName = c.ConsumerName,
                                Topic = topic,
                                GroupId = c.GroupId,
                                InstanceName = $"{Environment.MachineName}-{_processId}",
                                PausedPartitions = c.PausedPartitions
                                    .Where(p => p.Topic == topic)
                                    .Select(p => p.Partition.Value),
                                RunningPartitions = c.RunningPartitions
                                    .Where(p => p.Topic == topic)
                                    .Select(p => p.Partition.Value),
                                WorkersCount = c.WorkersCount,
                                Status = c.Status,
                                Lag = consumerLag.Where(l => l.Topic == topic).Sum(l => l.Lag),
                                SentAt = DateTime.UtcNow,
                            });
                    });

            foreach (var item in items)
            {
                producer.Produce(topicName, Guid.NewGuid().ToByteArray(), item, partition: partition);
            }
        }
        catch (Exception e)
        {
            _logHandler.Warning("Error producing telemetry data", new { Exception = e });
        }
    }
}