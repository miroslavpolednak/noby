using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace CIS.Infrastructure.Data.Redis;

public class RedisSubscriberConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<RedisSubscriberConsumer> _logger;
    private readonly IRedisConsumeService _redisConsumeService;
    private readonly RedisMessagingOptions _configuration;

    public RedisSubscriberConsumer(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<RedisSubscriberConsumer> logger,
        IOptions<RedisMessagingOptions> configuration,
        IRedisConsumeService redisConsumeService)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _redisConsumeService = redisConsumeService;
        _configuration = configuration.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();
        var provider = serviceScope.ServiceProvider;
        var subscriber = provider.GetRequiredService<ISubscriber<RedisMessage>>();
        try
        {
            await ConsumeMessages(subscriber);
        }
        catch (Exception exp)
        {
            _logger.UnknownConsumerException(exp);
        }
    }

    private async Task ConsumeMessages(ISubscriber<RedisMessage> subscriber)
    {
        var subConsumer = subscriber.GetSubscriber();
        await subConsumer.SubscribeAsync(RedisChannel.Pattern(_configuration.RedisChannel), async (channel, message) =>
        {
            await Task.Yield();

            try
            {
                await _redisConsumeService.ProcessRedisMessage(message, ConsumerFromSource.Channel);
            }
            catch (Exception exp)
            {
                _logger.UnknownConsumerException(exp);
            }
        });
    }
}
