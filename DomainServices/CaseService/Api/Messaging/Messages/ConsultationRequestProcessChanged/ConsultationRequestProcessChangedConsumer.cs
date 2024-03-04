using DomainServices.CaseService.Api.Services;
using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.ConsultationRequestProcessChanged;

internal sealed class ConsultationRequestProcessChangedConsumer
    : IConsumer<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ConsultationRequestProcessChanged>
{
    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ConsultationRequestProcessChanged> context)
    {
        var token = context.CancellationToken;
        var message = context.Message;
        
        if (!int.TryParse(message.currentTask.id, out var currentTaskId))
        {
            _logger.KafkaMessageCurrentTaskIdIncorrectFormat(nameof(ConsultationRequestProcessChangedConsumer), message.currentTask.id);
        }
        
        if (!long.TryParse(message.@case.caseId.id, out var caseId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(nameof(ConsultationRequestProcessChangedConsumer), message.@case.caseId.id);
        }
        
        await _activeTasksService.UpdateActiveTaskByTaskIdSb(caseId, currentTaskId, token);
    }

    private readonly ActiveTasksService _activeTasksService;
    private readonly ILogger<ConsultationRequestProcessChangedConsumer> _logger;

    public ConsultationRequestProcessChangedConsumer(
        ActiveTasksService activeTasksService,
        ILogger<ConsultationRequestProcessChangedConsumer> logger)
    {
        _activeTasksService = activeTasksService;
        _logger = logger;
    }
}