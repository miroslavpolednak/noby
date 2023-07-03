﻿using DomainServices.CodebookService.Clients;
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

        await replaceActiveTasks(request.CaseId, request.Tasks, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
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
                    TaskTypeId = t.TaskTypeId,
                    TaskIdSb = t.TaskIdSb
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
                e.TaskIdSb = tasksToUpdateById[e.TaskProcessId].TaskIdSb;
            });
        }

        await _dbContext.SaveChangesAsync(cancellation);
    }

    private readonly CaseServiceDbContext _dbContext;
    private readonly ICodebookServiceClient _codebookService;
    
    public UpdateActiveTasksHandler(
        CaseServiceDbContext dbContext,
        ICodebookServiceClient codebookService)
    {
        _dbContext = dbContext;
        _codebookService = codebookService;
    }
}
