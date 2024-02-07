using CIS.Core;
using Microsoft.Extensions.Options;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using StackExchange.Redis;
using System.Diagnostics;

namespace CIS.Infrastructure.Data.Redis;

public interface IPublisher<T> where T : class
{
    Task PublishAsync(T message, string channel = "");

    Task EnqueueAsync(T message, string queueId = "");
}

public class Publisher<T> : IPublisher<T> where T : RedisMessage
{
    private static readonly TextMapPropagator _propagator = new TraceContextPropagator();

    private readonly IConnectionMultiplexer _connection;
    private readonly TimeProvider _dateTime;
    private readonly ILogger<Publisher<T>> _logger;
    private readonly RedisMessagingOptions _configuration;

    public Publisher(
        IConnectionMultiplexer connection,
        TimeProvider dateTime,
        ILogger<Publisher<T>> logger,
        IOptions<RedisMessagingOptions> configuration)
    {
        _connection = connection;
        _dateTime = dateTime;
        _logger = logger;
        _configuration = configuration.Value;
    }

    public async Task EnqueueAsync(T message, string queueId = "")
    {
        ArgumentNullException.ThrowIfNull(message);

        queueId = string.IsNullOrWhiteSpace(queueId) ? _configuration.RedisQueueId : queueId;

        var telemetryInfo = GetTelemetryInfo();

        var type = message.GetType();
        message.MessageType = type.AssemblyQualifiedName!;
        message.CreatedAt = _dateTime.GetLocalNow().DateTime;
        message.TelemetryInfo = telemetryInfo;
        await _connection.EnqueueItem(queueId, message);
        _logger.MessageAddedToQueue(queueId, message.GetType().FullName!);
    }

    public async Task PublishAsync(T message, string channel = "")
    {
        channel = string.IsNullOrWhiteSpace(channel) ? _configuration.RedisChannel : channel;

        ArgumentNullException.ThrowIfNull(message);

        var telemetryInfo = GetTelemetryInfo();

        var type = message.GetType();
        message.MessageType = type.AssemblyQualifiedName!;
        message.CreatedAt = _dateTime.GetLocalNow().DateTime;
        message.TelemetryInfo = telemetryInfo;
        await _connection.Publish(channel, message);
        _logger.MessageAddedToChanel(channel, message.GetType().FullName!);
    }

    private static Dictionary<string, string> GetTelemetryInfo()
    {
        Dictionary<string, string> telemetryInfo = null!;
        var activityContext = Activity.Current?.Context;
        if (activityContext is not null)
        {
            telemetryInfo = [];
            _propagator.Inject(new PropagationContext(activityContext.Value, Baggage.Current), telemetryInfo, (d, key, value) => d.TryAdd(key, value));
        }

        return telemetryInfo;
    }
}
