using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.ConsultationRequestProcessChanged;

internal sealed class ConsultationRequestProcessChangedConsumer
    : IConsumer<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ConsultationRequestProcessChanged>
{
    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ConsultationRequestProcessChanged> context)
    {
        var token = context.CancellationToken;
        var message = context.Message;
        
        if (!int.TryParse(context.Message.currentTask.id, out var currentTaskId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(context.Message.@case.caseId.id);
        }
        
        if (!long.TryParse(context.Message.@case.caseId.id, out var caseId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(context.Message.@case.caseId.id);
        }
        
        await _activeTask.UpdateActiveTask(caseId, currentTaskId, token);
    }

    private readonly Services.ActiveTaskService _activeTask;
    private readonly ILogger<ConsultationRequestProcessChangedConsumer> _logger;

    public ConsultationRequestProcessChangedConsumer(
        Services.ActiveTaskService activeTask,
        ILogger<ConsultationRequestProcessChangedConsumer> logger)
    {
        _activeTask = activeTask;
        _logger = logger;
    }
}