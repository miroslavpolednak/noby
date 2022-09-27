using CIS.InternalServices.NotificationService.Msc;
using CIS.InternalServices.NotificationService.Msc.Messages;
using Confluent.Kafka;
using Microsoft.Extensions.Caching.Memory;

namespace CIS.InternalServices.NotificationService.Api.HostedServices;

public class MscResultConsumer : BackgroundService
{
    private readonly IConsumer<Null, MscResult> _mscResultConsumer;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<MscResultConsumer> _logger;
    
    public MscResultConsumer(
        IConsumer<Null, MscResult> mscResultConsumer,
        IMemoryCache memoryCache,
        ILogger<MscResultConsumer> logger)
    {
        _mscResultConsumer = mscResultConsumer;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(TimeSpan.Zero, stoppingToken);
        _logger.LogInformation("Subscribing topic: {topic}", Topics.MscResultIn);
        _mscResultConsumer.Subscribe(Topics.MscResultIn);

        await Task.Run(() =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = _mscResultConsumer.Consume(TimeSpan.FromSeconds(5));
                _logger.LogInformation("Received result: {result}", result);
                if (result != null)
                {
                    _memoryCache.Set(result.Message.Value, result.Message.Value);
                }
            }
        }, stoppingToken);

        // _mscResultConsumer.Unsubscribe();
        _mscResultConsumer.Close();
    }
}