using Microsoft.Extensions.Options;
using OpenTelemetry.Context.Propagation;
using StackExchange.Redis;
using System.Diagnostics;
using System.Text.Json;

namespace CIS.Infrastructure.Data.Redis;

public interface IRedisConsumeService
{
    Task ProcessRedisMessage(RedisValue value, ConsumerFromSource source);
}

public class RedisConsumeService : IRedisConsumeService
{
    private static readonly TextMapPropagator _propagator = new TraceContextPropagator();
    private static readonly ActivitySource _activitySource = new(nameof(RedisConsumeService));

    private readonly ILogger<RedisConsumeService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly RedisMessagingOptions _configuration;

    public RedisConsumeService(
        ILogger<RedisConsumeService> logger,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<RedisMessagingOptions> configuration)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _configuration = configuration.Value;


        // subscribe to listener
        ActivitySource.AddActivityListener(new()
        {
            ShouldListenTo = _ => true,
            SampleUsingParentId = (ref ActivityCreationOptions<string> _) => ActivitySamplingResult.AllData,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData,
        });
    }

    public async Task ProcessRedisMessage(RedisValue value, ConsumerFromSource source)
    {
        if (value.IsNullOrEmpty)
            throw new ArgumentNullException(nameof(value));

        var redisMessageBase = JsonSerializer.Deserialize<RedisMessage>(value!);

        if (redisMessageBase is null)
            _logger.UnknownMessage(value!);
        else
            await SendMessageToHandlers(value, redisMessageBase, source);
    }

    private async Task SendMessageToHandlers(RedisValue message, RedisMessage redisMessageBase, ConsumerFromSource source)
    {
        var contractMesageType = Type.GetType(redisMessageBase.MessageType);
        if (contractMesageType is null)
        {
            _logger.UnknownMessageForAssembly(redisMessageBase.MessageType);
        }
        else
        {
            var contractObj = (RedisMessage?)JsonSerializer.Deserialize(message!, contractMesageType);
            ArgumentNullException.ThrowIfNull(nameof(contractObj));

            // Set ActivityContext from paret (TraceId, SpanId, etc.)
            var parentContext = _propagator.Extract(default, redisMessageBase.TelemetryInfo, (d, _) => d.Select(r => r.Value));
            using var activity = _activitySource.StartActivity(contractMesageType.FullName!, ActivityKind.Consumer, parentContext.ActivityContext);

            // Process handlers
            using var handlersServiceScope = _serviceScopeFactory.CreateScope();
            var allHandlers = handlersServiceScope.ServiceProvider.GetServices<IHandler>();
            foreach (var handler in allHandlers.Where(r => r.CanHandle(contractObj!)))
            {
                _logger.StartProcessingMessage(GetSource(source), contractMesageType.FullName!, handler.GetType().FullName!);
                try
                {
                    await handler.Handle(contractObj!);
                }
                catch (Exception exp)
                {
                    _logger.UnknownConsumerException(exp);
                }

                _logger.EndProcessingMessage(GetSource(source), contractMesageType.FullName!, handler.GetType().FullName!);
            }
        }
    }

    private string GetSource(ConsumerFromSource consumerFromSource) => consumerFromSource switch
    {
        ConsumerFromSource.Queue => _configuration.RedisQueueId,
        ConsumerFromSource.Channel => _configuration.RedisChannel,
        _ => throw new NotSupportedException(),
    };

}
