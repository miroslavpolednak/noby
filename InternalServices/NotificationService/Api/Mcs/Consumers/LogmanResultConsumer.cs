using CIS.Infrastructure.Attributes;
using CIS.InternalServices.NotificationService.Api.Messaging.Consumers.Requests;
using CIS.InternalServices.NotificationService.Mcs;
using Confluent.Kafka;
using cz.kb.osbs.mcs.notificationreport.eventapi.v2.report;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Messaging.Consumers;

[ScopedService, SelfService]
public class LogmanResultConsumer
{
    private readonly IMediator _mediator;
    private readonly IConsumer<string, NotificationReport> _consumer;
    private readonly ILogger<LogmanResultConsumer> _logger;

    public LogmanResultConsumer(
        IMediator mediator,
        IConsumer<string, NotificationReport> consumer,
        ILogger<LogmanResultConsumer> logger)
    {
        _mediator = mediator;
        _consumer = consumer;
        _logger = logger;
    }
    
    public async Task ConsumeAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(Topics.McsResultIn);
        _logger.LogInformation($"Consumer '{nameof(LogmanResultConsumer)}': Topic '{Topics.McsResultIn}' subscribed.");

        await Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(stoppingToken);
                var request = new ResultConsumeRequest { Message = result.Message };
                await _mediator.Send(request, stoppingToken);
            }
        }, stoppingToken);

        _consumer.Close();
        _logger.LogInformation($"Consumer '{nameof(LogmanResultConsumer)}': closed.");
    }
}