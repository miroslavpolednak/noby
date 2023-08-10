using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.CaseService.Api.Services;

[CIS.Core.Attributes.TransientService, CIS.Core.Attributes.SelfService]
internal sealed class ActiveTaskService
{
    public async Task UpdateActiveTask(long caseId, int taskIdSb, CancellationToken cancellationToken)
    {
        _logger.UpdateActiveTaskStart(caseId, taskIdSb);
        var taskDetail = await _mediator.Send(new GetTaskDetailRequest { TaskIdSb = taskIdSb }, cancellationToken);
        var taskTypeId = taskDetail.TaskObject.TaskTypeId;
        
        var taskState = (await _codebookService.WorkflowTaskStates(cancellationToken))
            .FirstOrDefault(t => t.Id == taskDetail.TaskObject.StateIdSb);

        var isActive = (taskState?.Flag == WorkflowTaskStatesResponse.Types.WorkflowTaskStatesItem.Types.EWorkflowTaskStateFlag.None && taskTypeId != 2)
            || (taskTypeId == 2 && taskDetail.TaskObject.PhaseTypeId == 1);

        // pokud je jiz task ulozeny v DB
        var savedTask = await _dbContext.ActiveTasks
                .FirstOrDefaultAsync(t => t.TaskId == taskDetail.TaskObject.TaskId, cancellationToken);

        _logger.BeforeUpdateActiveTasks(isActive, savedTask is not null);
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
        else if (!isActive && savedTask is not null)
        {
            _dbContext.ActiveTasks.Remove(savedTask);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private readonly Database.CaseServiceDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ILogger<ActiveTaskService> _logger;

    public ActiveTaskService(
        IMediator mediator,
        ICodebookServiceClient codebookService,
        Database.CaseServiceDbContext dbContext,
        ILogger<ActiveTaskService> logger)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _codebookService = codebookService;
        _logger = logger;
    }
}