using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.CaseService.Api.Services;

[CIS.Core.Attributes.TransientService, CIS.Core.Attributes.SelfService]
internal sealed class LinkTaskToCaseService
{
    public async Task Link(long caseId, int taskIdSb, CancellationToken cancellationToken)
    {
        var taskDetail = await _mediator.Send(new GetTaskDetailRequest { TaskIdSb = taskIdSb }, cancellationToken);
        var taskTypeId = taskDetail.TaskObject.TaskTypeId;
        
        var taskState = (await _codebookService.WorkflowTaskStates(cancellationToken))
            .FirstOrDefault(t => t.Id == taskDetail.TaskObject.StateIdSb);

        var isActive = (taskState?.Flag == WorkflowTaskStatesResponse.Types.WorkflowTaskStatesItem.Types.EWorkflowTaskStateFlag.None && taskTypeId != 2)
            || (taskTypeId != 2 && taskDetail.TaskObject.PhaseTypeId == 1);

        // pokud je jiz task ulozeny v DB
        var savedTask = await _dbContext.ActiveTasks
                .FirstOrDefaultAsync(t => t.TaskId == taskDetail.TaskObject.TaskId, cancellationToken);

        if (isActive && savedTask is null)
        {
            _dbContext.ActiveTasks.Add(new Database.Entities.ActiveTask
            {
                TaskIdSb = taskIdSb,
                CaseId = caseId,
                TaskId = taskDetail.TaskObject.TaskId,
                TaskTypeId = taskTypeId
            });
        }
        else if (isActive && savedTask is not null)
        {
            // co mam updatovat???
        }
        else if (savedTask is not null)
        {
            _dbContext.ActiveTasks.Remove(savedTask);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private readonly Database.CaseServiceDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly ICodebookServiceClient _codebookService;

    public LinkTaskToCaseService(IMediator mediator, ICodebookServiceClient codebookService, Database.CaseServiceDbContext dbContext)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _codebookService = codebookService;
    }
}