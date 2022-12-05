using CIS.Infrastructure.Attributes;
using CIS.InternalServices.NotificationService.Api.Handlers.Email.Requests;
using Confluent.Kafka;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Services.Mcs.Consumers;

[ScopedService, SelfService]
public class BusinessSendEmailConsumer
{
    private readonly IMediator _mediator;
    private readonly IConsumer<string, string> _consumer;
    private readonly ILogger<BusinessSendEmailConsumer> _logger;

    public BusinessSendEmailConsumer(
        IMediator mediator,
        IConsumer<string, string> consumer,
        ILogger<BusinessSendEmailConsumer> logger)
    {
        _mediator = mediator;
        _consumer = consumer;
        _logger = logger;
    }
    
    public async Task ConsumeAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe("todo");
        _logger.LogInformation($"Consumer '{nameof(BusinessSendEmailConsumer)}': Topic '{"todo"}' subscribed.");
        
        await Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = _consumer.Consume(stoppingToken);
                    // todo:
                    var request = new SendEmailConsumeRequest {  };
                    await _mediator.Send(request, stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e,$"Consumer '{nameof(BusinessSendEmailConsumer)}': Consuming topic '{"todo"}' failed.");
                    throw;
                }
        
            }
        }, stoppingToken);
        
        _consumer.Close();
        _logger.LogInformation($"Consumer '{nameof(BusinessSendEmailConsumer)}': closed.");
    }
}