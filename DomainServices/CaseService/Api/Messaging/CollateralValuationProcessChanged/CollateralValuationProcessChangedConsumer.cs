using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.CollateralValuationProcessChanged;

internal sealed class CollateralValuationProcessChangedConsumer
    : IConsumer<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.CollateralValuationProcessChanged> 
{
    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.CollateralValuationProcessChanged> context)
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
    private readonly ILogger<CollateralValuationProcessChangedConsumer> _logger;

    public CollateralValuationProcessChangedConsumer(
        Services.ActiveTaskService activeTask,
        ILogger<CollateralValuationProcessChangedConsumer> logger)
    {
        _activeTask = activeTask;
        _logger = logger;
    }
}