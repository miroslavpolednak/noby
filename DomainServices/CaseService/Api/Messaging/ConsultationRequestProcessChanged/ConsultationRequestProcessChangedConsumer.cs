using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.ConsultationRequestProcessChanged;

public class ConsultationRequestProcessChangedConsumer
    : IConsumer<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ConsultationRequestProcessChanged>
{
    public Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ConsultationRequestProcessChanged> context)
    {
        var message = context.Message;
        var currentTaskId = int.Parse(message.currentTask.id, CultureInfo.InvariantCulture);
        return Task.CompletedTask;
    }
}