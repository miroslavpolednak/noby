﻿using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.Api.Database;
using DomainServices.CodebookService.Clients;

namespace DomainServices.CaseService.Api.Services;

[CIS.Core.Attributes.TransientService, CIS.Core.Attributes.AsImplementedInterfacesService]
internal sealed class ActiveTasksService 
    : IActiveTasksService
{
    public async Task SyncActiveTasks(long caseId, List<WorkflowTask> tasks, CancellationToken cancellation)
    {
        var taskStates = await _codebookService.WorkflowTaskStates(cancellation);
        var taskLookup = tasks.ToLookup(t => t.IsActive(taskStates));
        var activeTasks = taskLookup[true].ToList();
        var inactiveTasks = taskLookup[false].ToList();

        var entities = await _dbContext.ActiveTasks.Where(t => t.CaseId == caseId).ToListAsync(cancellation);
        var dictionary = entities.GroupBy(t => t.TaskId).ToDictionary(k => k.Key, v => v.ToList());

        // kontrola zdvojenych tasku
        foreach (var entry in dictionary.Where(t => t.Value.Count > 2))
        {
            _dbContext.ActiveTasks.RemoveRange(entry.Value.Where((task, index) => index > 0));
            entry.Value.RemoveRange(1, entry.Value.Count - 1);
        }

        // update and insert
        foreach (var activeTask in activeTasks)
        {
            if (dictionary.TryGetValue(activeTask.TaskId, out var entity))
            {
                // update
                entity[0].TaskTypeId = activeTask.TaskTypeId;
                entity[0].TaskIdSb = activeTask.TaskIdSb;
            }
            else
            {
                // insert
                _dbContext.ActiveTasks.Add(new Database.Entities.ActiveTask
                {
                    CaseId = caseId,
                    TaskId = activeTask.TaskId,
                    TaskTypeId = activeTask.TaskTypeId,
                    TaskIdSb = activeTask.TaskIdSb
                });
            }
        }

        // remove
        foreach (var inactiveTask in inactiveTasks)
        {
            if (dictionary.TryGetValue(inactiveTask.TaskId, out var entity))
            {
                _dbContext.ActiveTasks.Remove(entity[0]);
            }
        }

        await _dbContext.SaveChangesAsync(cancellation);
    }

    public async Task UpdateActiveTaskByTaskIdSb(long caseId, GetTaskDetailResponse taskDetail, CancellationToken cancellationToken)
    {
        _logger.UpdateActiveTaskStart(caseId, taskDetail.TaskObject.TaskIdSb);

        var taskTypeId = taskDetail.TaskObject.TaskTypeId;

        var taskStates = await _codebookService.WorkflowTaskStates(cancellationToken);
        var isActive = taskDetail.TaskObject.IsActive(taskStates);

        var entity = await _dbContext.ActiveTasks
            .FirstOrDefaultAsync(t => t.TaskId == taskDetail.TaskObject.TaskId, cancellationToken);

        _logger.BeforeUpdateActiveTasks(isActive, entity is not null);
        if (isActive && entity is null)
        {
            _dbContext.ActiveTasks.Add(new Database.Entities.ActiveTask
            {
                TaskIdSb = taskDetail.TaskObject.TaskIdSb,
                CaseId = caseId,
                TaskId = taskDetail.TaskObject.TaskId,
                TaskTypeId = taskTypeId
            });
        }

        if (!isActive && entity is not null)
        {
            _dbContext.ActiveTasks.Remove(entity);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateActiveTaskByTaskIdSb(long caseId, int taskIdSb, CancellationToken cancellationToken)
    {
        var taskDetail = await _mediator.Send(new GetTaskDetailRequest { TaskIdSb = taskIdSb }, cancellationToken);
        await UpdateActiveTaskByTaskIdSb(caseId, taskDetail, cancellationToken);
    }

    private readonly IMediator _mediator;
    private readonly CaseServiceDbContext _dbContext;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ILogger<ActiveTasksService> _logger;

    public ActiveTasksService(
        IMediator mediator,
        CaseServiceDbContext dbContext,
        ICodebookServiceClient codebookService,
        ILogger<ActiveTasksService> logger)
    {
        _mediator = mediator;
        _dbContext = dbContext;
        _codebookService = codebookService;
        _logger = logger;
    }
}
