using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.CollateralValuationProcessChanged;

internal sealed class CollateralValuationProcessChangedConsumer
    : IConsumer<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.CollateralValuationProcessChanged> 
{
    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.CollateralValuationProcessChanged> context)
    {
        var token = context.CancellationToken;
        var message = context.Message;
        
        var currentTaskId = int.Parse(message.currentTask.id, CultureInfo.InvariantCulture);
        await _linkTaskToCase.Link(currentTaskId, token);
    }

    private readonly Services.LinkTaskToCaseService _linkTaskToCase;

    public CollateralValuationProcessChangedConsumer(Services.LinkTaskToCaseService linkTaskToCase)
    {
        _linkTaskToCase = linkTaskToCase;
    }
}