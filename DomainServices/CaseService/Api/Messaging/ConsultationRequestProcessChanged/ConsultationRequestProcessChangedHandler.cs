using DomainServices.CaseService.Api.Services;
using KafkaFlow;

namespace DomainServices.CaseService.Api.Messaging.MessageHandlers;

internal class ConsultationRequestProcessChangedHandler(
	IActiveTasksService _activeTasksService,
	ILogger<ConsultationRequestProcessChangedHandler> _logger) 
    : IMessageHandler<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ConsultationRequestProcessChanged>
{
	public async Task Handle(IMessageContext context, cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ConsultationRequestProcessChanged message)
    {
        _logger.TempMessageHeaderLog(context, message.eventId, message.state.ToString());

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