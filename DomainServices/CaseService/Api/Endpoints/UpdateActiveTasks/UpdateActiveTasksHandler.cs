using DomainServices.CodebookService.Clients;
using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.Api.Database;
using Google.Protobuf.Collections;

namespace DomainServices.CaseService.Api.Endpoints.UpdateActiveTasks;

internal sealed class UpdateActiveTasksHandler
    : IRequestHandler<UpdateActiveTasksRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateActiveTasksRequest request, CancellationToken cancellation)
    {
        // check if case exists
        var entity = await _dbContext.Cases.FindAsync(new object[] { request.CaseId }, cancellation)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);

        // load WorkflowTaskTypes
        var taskTypes = await _codebookService.WorkflowTaskTypes(cancellation);
        var taskTypeIds = taskTypes.Select(i => i.Id).ToArray();

        // CheckTasks
        CheckTasks(request.Tasks, taskTypeIds);

        await replaceActiveTasks(request.CaseId, request.Tasks, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private static void CheckTasks(RepeatedField<ActiveTask> tasks, int[] taskTypeIds)
    {
        var tasksWithInvalidTypeId = tasks.Where(t => !taskTypeIds.Contains(t.TaskTypeId));

        if (!tasksWithInvalidTypeId.Any())
            return;

        var invalidTypeIds = tasksWithInvalidTypeId.Select(t => t.TaskTypeId).Distinct();
        var taskIds = tasksWithInvalidTypeId.Select(t => t.TaskId);

        throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.WfTaskValidationFailed1, string.Join(",", taskIds));
    }

    public async Task replaceActiveTasks(long caseId, RepeatedField<ActiveTask> tasks, CancellationToken cancellation)
    {
        var taskProcessIds = tasks.Select(i => i.TaskId);

        var entities = _dbContext.ActiveTasks.Where(i => i.CaseId == caseId);
        var entitiesIds = entities.Select(i => i.TaskProcessId);

        var idsToAdd = taskProcessIds.Where(id => !entitiesIds.Contains(id)).ToList();
        var idsToRemove = entitiesIds.Where(id => !taskProcessIds.Contains(id)).ToList();
        var idsToUpdate = entitiesIds.Where(id => !idsToAdd.Contains(id) && !idsToRemove.Contains(id)).ToList();

        // remove
        if (idsToRemove.Any())
        {
            _dbContext.ActiveTasks.RemoveRange(entities.Where(e => idsToRemove.Contains(e.TaskProcessId)));
        }

        // add
        if (idsToAdd.Any())
        {
            _dbContext.ActiveTasks.AddRange(
                tasks.Where(t => idsToAdd.Contains(t.TaskId)).Select(t => new Database.Entities.ActiveTask 
                { 
                    CaseId = caseId, 
                    TaskProcessId = t.TaskId, 
                    TaskTypeId = t.TaskTypeId 
                })
            );
        }

        // update
        if (idsToUpdate.Any())
        {
            var tasksToUpdateById = tasks.Where(t => idsToUpdate.Contains(t.TaskId)).ToDictionary(t => t.TaskId);
            entities.Where(e => idsToUpdate.Contains(e.TaskProcessId)).ToList().ForEach(e =>
            {
                e.TaskTypeId = tasksToUpdateById[e.TaskProcessId].TaskTypeId;
            });
        }

        await _dbContext.SaveChangesAsync(cancellation);
    }

    private readonly CaseServiceDbContext _dbContext;
    private readonly ICodebookServiceClients _codebookService;
    
    public UpdateActiveTasksHandler(
        CaseServiceDbContext dbContext,
        ICodebookServiceClients codebookService)
    {
        _dbContext = dbContext;
        _codebookService = codebookService;
    }
}
