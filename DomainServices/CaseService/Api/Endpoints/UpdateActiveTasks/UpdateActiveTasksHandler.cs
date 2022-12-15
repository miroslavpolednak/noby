using DomainServices.CodebookService.Clients;
using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.Api.Database;
using Microsoft.EntityFrameworkCore;
using Google.Protobuf.Collections;

namespace DomainServices.CaseService.Api.Endpoints.UpdateActiveTasks;

internal sealed class UpdateActiveTasksHandler
    : IRequestHandler<UpdateActiveTasksRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateActiveTasksRequest request, CancellationToken cancellation)
    {
        // check if case exists
        var entity = await _dbContext.Cases.FindAsync(new object[] { request.CaseId }, cancellation)
            ?? throw new CisNotFoundException(13000, "Case", request.CaseId);

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
        var tasksWithInvalidTypeId = tasks.Where(t => !taskTypeIds.Contains(t.TypeId));

        if (!tasksWithInvalidTypeId.Any())
            return;

        var invalidTypeIds = tasksWithInvalidTypeId.Select(t => t.TypeId).Distinct();
        var taskIds = tasksWithInvalidTypeId.Select(t => t.TaskProcessId);

        throw new CisValidationException(13007, $"Found tasks [{string.Join(",", taskIds)}] with invalid TypeId [{string.Join(",", invalidTypeIds)}].");
    }

    public async Task replaceActiveTasks(long caseId, RepeatedField<ActiveTask> tasks, CancellationToken cancellation)
    {
        var taskProcessIds = tasks.Select(i => i.TaskProcessId);

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
                tasks.Where(t => idsToAdd.Contains(t.TaskProcessId)).Select(t => new Database.Entities.ActiveTask 
                { 
                    CaseId = caseId, 
                    TaskProcessId = t.TaskProcessId, 
                    TaskTypeId = t.TypeId 
                })
            );
        }

        // update
        if (idsToUpdate.Any())
        {
            var tasksToUpdateById = tasks.Where(t => idsToUpdate.Contains(t.TaskProcessId)).ToDictionary(t => t.TaskProcessId);
            entities.Where(e => idsToUpdate.Contains(e.TaskProcessId)).ToList().ForEach(e =>
            {
                e.TaskTypeId = tasksToUpdateById[e.TaskProcessId].TypeId;
            });
        }

        await _dbContext.SaveChangesAsync(cancellation);
    }

    private readonly CaseServiceDbContext _dbContext;
    private readonly ICodebookServiceClients _codebookService;
    private readonly UserService.Clients.IUserServiceClient _userService;
    private readonly EasSimulationHT.IEasSimulationHTClient _easSimulationHTClient;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;

    public UpdateActiveTasksHandler(
        CaseServiceDbContext dbContext,
        ICodebookServiceClients codebookService,
        UserService.Clients.IUserServiceClient userService,
        EasSimulationHT.IEasSimulationHTClient easSimulationHTClient,
        CIS.Core.Security.ICurrentUserAccessor userAccessor
        )
    {
        _dbContext = dbContext;
        _codebookService = codebookService;
        _userService = userService;
        _easSimulationHTClient = easSimulationHTClient;
        _userAccessor = userAccessor;
    }
}
