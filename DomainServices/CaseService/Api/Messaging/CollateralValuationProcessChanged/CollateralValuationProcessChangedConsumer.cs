using DomainServices.CaseService.Api.Services;
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
        
        await _activeTasksService.UpdateActiveTaskByTaskIdSb(caseId, currentTaskId, token);
    }

    private readonly ActiveTasksService _activeTasksService;
    private readonly ILogger<CollateralValuationProcessChangedConsumer> _logger;

    public CollateralValuationProcessChangedConsumer(
        ActiveTasksService activeTasksService,
        ILogger<CollateralValuationProcessChangedConsumer> logger)
    {
        _activeTasksService = activeTasksService;
        _logger = logger;
    }
}