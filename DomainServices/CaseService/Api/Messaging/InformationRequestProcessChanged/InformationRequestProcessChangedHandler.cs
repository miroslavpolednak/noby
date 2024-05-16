using DomainServices.CaseService.Api.Services;
using KafkaFlow;

namespace DomainServices.CaseService.Api.Messaging.MessageHandlers;

internal class InformationRequestProcessChangedHandler(
	ActiveTasksService _activeTasksService,
	ILogger<InformationRequestProcessChangedHandler> _logger) 
    : IMessageHandler<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.InformationRequestProcessChanged>
{
	public async Task Handle(IMessageContext context, cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.InformationRequestProcessChanged message)
    {
        if (!int.TryParse(message.currentTask.id, out var currentTaskId))
        {
            _logger.KafkaMessageCurrentTaskIdIncorrectFormat(nameof(InformationRequestProcessChangedHandler), message.currentTask.id);
        }
        
        if (!long.TryParse(message.@case.caseId.id, out var caseId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(nameof(InformationRequestProcessChangedHandler), message.@case.caseId.id);
        }
        
        await _activeTasksService.UpdateActiveTaskByTaskIdSb(caseId, currentTaskId, CancellationToken.None);
    }
}