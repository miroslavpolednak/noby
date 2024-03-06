using DomainServices.CaseService.Api.Services;
using KafkaFlow;

namespace DomainServices.CaseService.Api.Messaging.MessageHandlers;

internal class CollateralValuationProcessChangedHandler : IMessageHandler<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.CollateralValuationProcessChanged>
{
    private readonly ActiveTasksService _activeTasksService;
    private readonly ILogger<CollateralValuationProcessChangedHandler> _logger;

    public CollateralValuationProcessChangedHandler(
        ActiveTasksService activeTasksService,
        ILogger<CollateralValuationProcessChangedHandler> logger)
    {
        _activeTasksService = activeTasksService;
        _logger = logger;
    }

    public async Task Handle(IMessageContext context, cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.CollateralValuationProcessChanged message)
    {
        if (!int.TryParse(message.currentTask.id, out var currentTaskId))
        {
            _logger.KafkaMessageCurrentTaskIdIncorrectFormat(nameof(CollateralValuationProcessChangedHandler), message.currentTask.id);
        }
        
        if (!long.TryParse(message.@case.caseId.id, out var caseId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(nameof(CollateralValuationProcessChangedHandler), message.@case.caseId.id);
        }
        
        await _activeTasksService.UpdateActiveTaskByTaskIdSb(caseId, currentTaskId, CancellationToken.None);
    }
}