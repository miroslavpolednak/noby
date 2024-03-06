using DomainServices.CaseService.Api.Services;
using KafkaFlow;

namespace DomainServices.CaseService.Api.Messaging.MessageHandlers;

internal class ConsultationRequestProcessChangedHandler : IMessageHandler<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ConsultationRequestProcessChanged>
{
    private readonly ActiveTasksService _activeTasksService;
    private readonly ILogger<ConsultationRequestProcessChangedHandler> _logger;

    public ConsultationRequestProcessChangedHandler(
        ActiveTasksService activeTasksService,
        ILogger<ConsultationRequestProcessChangedHandler> logger)
    {
        _activeTasksService = activeTasksService;
        _logger = logger;
    }

    public async Task Handle(IMessageContext context, cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ConsultationRequestProcessChanged message)
    {
        if (!int.TryParse(message.currentTask.id, out var currentTaskId))
        {
            _logger.KafkaMessageCurrentTaskIdIncorrectFormat(nameof(ConsultationRequestProcessChangedHandler), message.currentTask.id);
        }
        
        if (!long.TryParse(message.@case.caseId.id, out var caseId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(nameof(ConsultationRequestProcessChangedHandler), message.@case.caseId.id);
        }
        
        await _activeTasksService.UpdateActiveTaskByTaskIdSb(caseId, currentTaskId, CancellationToken.None);
    }
}