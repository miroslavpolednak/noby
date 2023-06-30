using DomainServices.CaseService.Api.Endpoints.LinkTaskToCase;
using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.ConsultationRequestProcessChanged;

public class ConsultationRequestProcessChangedConsumer
    : IConsumer<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ConsultationRequestProcessChanged>
{
    private readonly IMediator _mediator;

    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ConsultationRequestProcessChanged> context)
    {
        var token = context.CancellationToken;
        var message = context.Message;
        
        var currentTaskId = int.Parse(message.currentTask.id, CultureInfo.InvariantCulture);
        await _mediator.Send(new LinkTaskToCaseRequest{ TaskId = currentTaskId }, token);
    }

    public ConsultationRequestProcessChangedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
}