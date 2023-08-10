using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.CollateralValuationProcessChanged;

internal sealed class CollateralValuationProcessChangedConsumer
    : IConsumer<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.CollateralValuationProcessChanged> 
{
    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.CollateralValuationProcessChanged> context)
    {
        var token = context.CancellationToken;
        var message = context.Message;
        
        var currentTaskId = int.Parse(message.currentTask.id, CultureInfo.InvariantCulture);
        var caseId = long.Parse(message.@case.caseId.id, CultureInfo.InvariantCulture);
        await _activeTask.UpdateActiveTask(caseId, currentTaskId, token);
    }

    private readonly Services.ActiveTaskService _activeTask;

    public CollateralValuationProcessChangedConsumer(Services.ActiveTaskService activeTask)
    {
        _activeTask = activeTask;
    }
}