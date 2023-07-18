using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.ConsultationRequestProcessChanged;

internal sealed class ConsultationRequestProcessChangedConsumer
    : IConsumer<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ConsultationRequestProcessChanged>
{
    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ConsultationRequestProcessChanged> context)
    {
        var token = context.CancellationToken;
        var message = context.Message;
        
        var currentTaskId = int.Parse(message.currentTask.id, CultureInfo.InvariantCulture);
        var caseId = long.Parse(message.@case.caseId.id, CultureInfo.InvariantCulture);

        await _linkTaskToCase.Link(caseId, currentTaskId, token);
    }

    private readonly Services.LinkTaskToCaseService _linkTaskToCase;

    public ConsultationRequestProcessChangedConsumer(Services.LinkTaskToCaseService linkTaskToCase)
    {
        _linkTaskToCase = linkTaskToCase;
    }
}