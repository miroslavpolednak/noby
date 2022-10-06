using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using CIS.InternalServices.NotificationService.Msc;
using Confluent.Kafka;
using cz.kb.osbs.mcs.notificationreport.eventapi.v2.report;
using Microsoft.Extensions.Caching.Memory;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CIS.InternalServices.NotificationService.Api.HostedServices;

public class MscResultConsumer : BackgroundService
{
    private readonly IConsumer<Null, NotificationReport> _mscResultConsumer;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<MscResultConsumer> _logger;
    
    public MscResultConsumer(
        IConsumer<Null, NotificationReport> mscResultConsumer,
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
                var notificationReport = result.Message.Value;
                _logger.LogInformation("Received report: {report}", JsonSerializer.Serialize(notificationReport));

                if (!_memoryCache.TryGetValue(notificationReport.id, out ResultGetResponse resultResponse)) continue;
                
                resultResponse.State = NotificationState.Delivered;
                _memoryCache.Set(notificationReport.id, resultResponse);
            }
            
            _mscResultConsumer.Close();
        }, stoppingToken);
    }
}