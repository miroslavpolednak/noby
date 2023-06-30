using DomainServices.CaseService.Api.Endpoints.LinkTaskToCase;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.CollateralValuationProcessChanged;

public class CollateralValuationProcessChangedConsumer
    : IConsumer<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.CollateralValuationProcessChanged> 
{
    private readonly IMediator _mediator;
    
    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.CollateralValuationProcessChanged> context)
    {
        var token = context.CancellationToken;
        var message = context.Message;
        
        var currentTaskId = int.Parse(message.currentTask.id, CultureInfo.InvariantCulture);
        await _mediator.Send(new LinkTaskToCaseRequest{ TaskId = currentTaskId }, token);
    }

    public CollateralValuationProcessChangedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
}