using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Repositories;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using CIS.InternalServices.NotificationService.Msc;
using Confluent.Kafka;
using cz.kb.osbs.mcs.notificationreport.eventapi.v2.report;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CIS.InternalServices.NotificationService.Api.HostedServices;

public class MscResultConsumer : BackgroundService
{
    private readonly IConsumer<Null, NotificationReport> _mscResultConsumer;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MscResultConsumer> _logger;
    
    public MscResultConsumer(
        IConsumer<Null, NotificationReport> mscResultConsumer,
        IServiceProvider serviceProvider,
        ILogger<MscResultConsumer> logger)
    {
        _mscResultConsumer = mscResultConsumer;
        _serviceProvider = serviceProvider;
        _logger = logger;
        
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Subscribing topic: {topic}", Topics.MscResultIn);
        _mscResultConsumer.Subscribe(Topics.MscResultIn);
        
        await Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = _mscResultConsumer.Consume(stoppingToken);
                var notificationReport = result.Message.Value;
                _logger.LogInformation("Received report: {report}", JsonSerializer.Serialize(notificationReport));

                if (!Guid.TryParse(notificationReport.id, out var notificationId))
                {
                    _logger.LogError("Could not parse notificationId: {id}", notificationReport.id);
                    continue;
                }
                
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var repository = scope.ServiceProvider.GetRequiredService<NotificationRepository>();
                    await repository.UpdateResult(
                        notificationId,
                        NotificationState.Delivered,
                        new HashSet<string>(),
                        stoppingToken);
                }
                catch (CisNotFoundException)
                {
                }
                catch
                {
                    _logger.LogError("Could not process report {report}", JsonSerializer.Serialize(notificationReport));
                }

            }
            
            _mscResultConsumer.Close();
        }, stoppingToken);
    }
}