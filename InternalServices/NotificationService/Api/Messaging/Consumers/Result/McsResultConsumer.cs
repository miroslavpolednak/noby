using cz.kb.osbs.mcs.notificationreport.eventapi.v3.report;
using MassTransit;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Messaging.Consumers.Result;

public class McsResultConsumer : IConsumer<NotificationReport>
{
    private readonly IMediator _mediator;

    public McsResultConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<NotificationReport> context)
    {
        var request = new ConsumeResultRequest { NotificationReport = context.Message };
        await _mediator.Send(request);
    }
}