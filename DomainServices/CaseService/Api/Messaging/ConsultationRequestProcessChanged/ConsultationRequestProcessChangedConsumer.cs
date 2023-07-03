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
        await _linkTaskToCase.Link(currentTaskId, token);
    }

    private readonly Services.LinkTaskToCaseService _linkTaskToCase;

    public ConsultationRequestProcessChangedConsumer(Services.LinkTaskToCaseService linkTaskToCase)
    {
        _linkTaskToCase = linkTaskToCase;
    }
}