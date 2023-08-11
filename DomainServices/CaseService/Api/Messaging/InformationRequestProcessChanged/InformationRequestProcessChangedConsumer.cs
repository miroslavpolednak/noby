using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.InformationRequestProcessChanged;

internal sealed class InformationRequestProcessChangedConsumer
    : IConsumer<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.InformationRequestProcessChanged>
{
    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.InformationRequestProcessChanged> context)
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
    private readonly ILogger<InformationRequestProcessChangedConsumer> _logger;

    public InformationRequestProcessChangedConsumer(
        Services.ActiveTaskService activeTask,
        ILogger<InformationRequestProcessChangedConsumer> logger)
    {
        _activeTask = activeTask;
        _logger = logger;
    }
}