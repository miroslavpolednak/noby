using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using CIS.InternalServices.NotificationService.Msc;
using CIS.InternalServices.NotificationService.Msc.Messages;
using Confluent.Kafka;
using Microsoft.Extensions.Caching.Memory;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CIS.InternalServices.NotificationService.Api.HostedServices;

public class MscResultConsumer : BackgroundService
{
    // todo: replace string with Msc contract
    private readonly IConsumer<Null, string> _mscResultConsumer;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<MscResultConsumer> _logger;
    
    public MscResultConsumer(
        IConsumer<Null, string> mscResultConsumer,
        IMemoryCache memoryCache,
        ILogger<MscResultConsumer> logger)
    {
        _mscResultConsumer = mscResultConsumer;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Subscribing topic: {topic}", Topics.MscResultIn);
        _mscResultConsumer.Subscribe(Topics.MscResultIn);

        await Task.Run(() =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = _mscResultConsumer.Consume(stoppingToken);
                _logger.LogInformation("Received result: {result}", result);
                var mscResult = JsonSerializer.Deserialize<MscResult>(result.Message.Value)!;

                if (!_memoryCache.TryGetValue(mscResult.NotificationId, out ResultGetResponse resultResponse)) continue;
                
                resultResponse.State = NotificationState.Delivered;
                _memoryCache.Set(mscResult.NotificationId, resultResponse);
            }
            
            _mscResultConsumer.Close();
        }, stoppingToken);
    }
}