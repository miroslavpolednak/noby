using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace CIS.Infrastructure.Data.Redis;

public class RedisQueueConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<RedisQueueConsumer> _logger;
    private readonly IRedisConsumeService _redisConsumeService;
    private readonly RedisMessagingOptions _configuration;

    public RedisQueueConsumer(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<RedisQueueConsumer> logger,
        IOptions<RedisMessagingOptions> configuration,
        IRedisConsumeService redisConsumeService)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _redisConsumeService = redisConsumeService;
        _configuration = configuration.Value;

        // subscribe to listener
        ActivitySource.AddActivityListener(new()
        {
            ShouldListenTo = _ => true,
            SampleUsingParentId = (ref ActivityCreationOptions<string> _) => ActivitySamplingResult.AllData,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData,
        });
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();
        var provider = serviceScope.ServiceProvider;
        var subscriber = provider.GetRequiredService<ISubscriber<RedisMessage>>();
        await ConsumeMessages(subscriber, stoppingToken);
    }

    private async Task ConsumeMessages(ISubscriber<RedisMessage> subscriber, CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Yield();

                var redisValue = await subscriber.DequeueAsync(_configuration.RedisQueueId);
                if (redisValue is not null)
                {
                    await _redisConsumeService.ProcessRedisMessage(redisValue.Value, ConsumerFromSource.Queue);
                }
                await Task.Delay(1000, stoppingToken);
            }
            catch (Exception exp)
            {
                _logger.UnknownConsumerException(exp);
            }
        }
    }
}
