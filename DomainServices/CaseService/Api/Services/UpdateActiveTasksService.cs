using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.Api.Database;

namespace DomainServices.CaseService.Api.Services;

[CIS.Core.Attributes.TransientService, CIS.Core.Attributes.SelfService]
internal sealed class UpdateActiveTasksService
{
    public async Task UpdateCaseTasks(long caseId, List<ActiveTask> tasks, CancellationToken cancellation)
    {
        var taskProcessIds = tasks.Select(i => i.TaskId);

        var entities = _dbContext.ActiveTasks.Where(i => i.CaseId == caseId);
        var entitiesIds = entities.Select(i => i.TaskId);

        var idsToAdd = taskProcessIds.Where(id => !entitiesIds.Contains(id)).ToList();
        var idsToRemove = entitiesIds.Where(id => !taskProcessIds.Contains(id)).ToList();
        var idsToUpdate = entitiesIds.Where(id => !idsToAdd.Contains(id) && !idsToRemove.Contains(id)).ToList();

        // remove
        if (idsToRemove.Any())
        {
            _dbContext.ActiveTasks.RemoveRange(entities.Where(e => idsToRemove.Contains(e.TaskId)));
        }

        // add
        if (idsToAdd.Any())
        {
            _dbContext.ActiveTasks.AddRange(
                tasks.Where(t => idsToAdd.Contains(t.TaskId)).Select(t => new Database.Entities.ActiveTask
                {
                    CaseId = caseId,
                    TaskId = t.TaskId,
                    TaskTypeId = t.TaskTypeId,
                    TaskIdSb = t.TaskIdSb
                })
            );
        }

        // update
        if (idsToUpdate.Any())
        {
            var tasksToUpdateById = tasks.Where(t => idsToUpdate.Contains(t.TaskId)).ToDictionary(t => t.TaskId);
            entities.Where(e => idsToUpdate.Contains(e.TaskId)).ToList().ForEach(e =>
            {
                e.TaskTypeId = tasksToUpdateById[e.TaskId].TaskTypeId;
                e.TaskIdSb = tasksToUpdateById[e.TaskId].TaskIdSb;
            });
        }

        await _dbContext.SaveChangesAsync(cancellation);
    }

    private readonly CaseServiceDbContext _dbContext;

    public UpdateActiveTasksService(CaseServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
