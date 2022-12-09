using CIS.InternalServices.NotificationService.Api.Handlers.Result.Requests;
using cz.kb.osbs.mcs.notificationreport.eventapi.v3.report;
using MassTransit;
using MassTransit.Mediator;

namespace CIS.InternalServices.NotificationService.Api.Services.Mcs.Consumers;

public class ResultConsumer : IConsumer<NotificationReport>
{
    private readonly IMediator _mediator;

    public ResultConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<NotificationReport> context)
    {
        var request = new ResultConsumeRequest { NotificationReport = context.Message };
        await _mediator.Send(request);
    }
}