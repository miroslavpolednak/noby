using DomainServices.CaseService.Api.Endpoints.LinkTaskToCase;
using MassTransit;
using IMediator = MassTransit.Mediator.IMediator;

namespace DomainServices.CaseService.Api.Messaging.InformationRequestProcessChanged;

public class InformationRequestProcessChangedConsumer
    : IConsumer<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.InformationRequestProcessChanged>
{
    private readonly IMediator _mediator;

    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.InformationRequestProcessChanged> context)
    {
        var token = context.CancellationToken;
        var message = context.Message;
        
        var currentTaskId = int.Parse(message.currentTask.id, CultureInfo.InvariantCulture);
        await _mediator.Send(new LinkTaskToCaseRequest{ TaskId = currentTaskId }, token);
    }

    public InformationRequestProcessChangedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
}