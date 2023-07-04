using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.InformationRequestProcessChanged;

internal sealed class InformationRequestProcessChangedConsumer
    : IConsumer<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.InformationRequestProcessChanged>
{
    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.InformationRequestProcessChanged> context)
    {
        var token = context.CancellationToken;
        var message = context.Message;
        
        var currentTaskId = int.Parse(message.currentTask.id, CultureInfo.InvariantCulture);
        var caseId = long.Parse(message.@case.caseId.id, CultureInfo.InvariantCulture);

        await _linkTaskToCase.Link(caseId, currentTaskId, token);
    }

    private readonly Services.LinkTaskToCaseService _linkTaskToCase;

    public InformationRequestProcessChangedConsumer(Services.LinkTaskToCaseService linkTaskToCase)
    {
        _linkTaskToCase = linkTaskToCase;
    }
}