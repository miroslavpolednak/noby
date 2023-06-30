using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.CaseService.Api.Endpoints.LinkTaskToCase;

public class LinkTaskToCaseHandler : IRequestHandler<LinkTaskToCaseRequest>
{
    private readonly IMediator _mediator;
    private readonly ICodebookServiceClient _codebookService;
    
    public async Task Handle(LinkTaskToCaseRequest request, CancellationToken cancellationToken)
    {
        var taskDetail = await _mediator.Send(new GetTaskDetailRequest { TaskIdSb = request.TaskId }, cancellationToken);

        var stateIdSb = taskDetail.TaskObject.StateIdSb;
        var taskTypeId = taskDetail.TaskObject.TaskTypeId;
        var phaseTypeId = taskDetail.TaskObject.PhaseTypeId;

        var taskStates = await _codebookService.WorkflowTaskStates(cancellationToken);
        var taskState = taskStates.FirstOrDefault(t => t.Id == stateIdSb);

        var isActive = taskState?.Flag == WorkflowTaskStatesResponse.Types.WorkflowTaskStatesItem.Types.EWorkflowTaskStateFlag.None;

        if (isActive && taskTypeId != 2 || taskTypeId == 2 && phaseTypeId == 1)
        {
            var taskList = await _mediator.Send(new GetTaskListRequest(), cancellationToken);
            var tasksIds = taskList.Tasks.Select(t => t.TaskId).ToHashSet();
            
        }
        else
        {
            
        }
    }

    public LinkTaskToCaseHandler(IMediator mediator, ICodebookServiceClient codebookService)
    {
        _mediator = mediator;
        _codebookService = codebookService;
    }
}