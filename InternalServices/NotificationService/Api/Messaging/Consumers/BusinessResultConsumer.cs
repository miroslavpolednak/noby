using CIS.Infrastructure.Attributes;
using CIS.InternalServices.NotificationService.Api.Messaging.Consumers.Requests;
using CIS.InternalServices.NotificationService.Mcs;
using Confluent.Kafka;
using cz.kb.osbs.mcs.notificationreport.eventapi.v2.report;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Messaging.Consumers;

[ScopedService, SelfService]
public class BusinessResultConsumer
{
    private readonly IMediator _mediator;
    private readonly IConsumer<string, NotificationReport> _consumer;
    private readonly ILogger<BusinessResultConsumer> _logger;

    public BusinessResultConsumer(
        IMediator mediator,
        IConsumer<string, NotificationReport> consumer,
        ILogger<BusinessResultConsumer> logger)
    {
        _mediator = mediator;
        _consumer = consumer;
        _logger = logger;
    }
    
    public async Task ConsumeAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(Topics.MscResultIn);
        _logger.LogInformation($"Consumer '{nameof(BusinessResultConsumer)}': Topic '{Topics.MscResultIn}' subscribed.");

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
        _logger.LogInformation($"Consumer '{nameof(BusinessResultConsumer)}': closed.");
    }
}