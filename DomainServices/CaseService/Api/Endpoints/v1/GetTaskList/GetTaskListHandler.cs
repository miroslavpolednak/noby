﻿using DomainServices.CaseService.Api.Services;
using DomainServices.CaseService.Contracts;
using ExternalServices.SbWebApi.V1;
using DomainServices.CaseService.Api.Database;
using ExternalServices.SbWebApi.Dto.FindTasks;
using DomainServices.CodebookService.Clients;

namespace DomainServices.CaseService.Api.Endpoints.v1.GetTaskList;

internal sealed class GetTaskListHandler(
    CaseServiceDbContext _dbContext,
    ISbWebApiClient _sbWebApiClient,
    IActiveTasksService _activeTasks,
    ICodebookServiceClient _codebookService)
        : IRequestHandler<GetTaskListRequest, GetTaskListResponse>
{
    public async Task<GetTaskListResponse> Handle(GetTaskListRequest request, CancellationToken cancellationToken)
    {
        // check if case exists
        if (!await _dbContext.Cases.AnyAsync(c => c.CaseId == request.CaseId, cancellationToken))
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);
        }

        // load codebooks
        var taskStateIds = (await _codebookService.WorkflowTaskStates(cancellationToken))
            .Select(i => i.Id)
            .ToList();

        // call SB
        var sbRequest = new FindByCaseIdRequest
        {
            CaseId = request.CaseId,
            TaskStates = taskStateIds,
            SearchPattern = "LoanProcessSubtasks"
        };
        var result = await _sbWebApiClient.FindTasksByCaseId(sbRequest, cancellationToken);

        var tasks = result
            .Where(t => Helpers.AllowedTaskTypeId.Contains(int.Parse(t["ukol_typ_noby"], CultureInfo.InvariantCulture)))
            .Select(taskData => taskData.ToWorkflowTask())
            .ToList();

        // update active tasks
        await _activeTasks.SyncActiveTasks(request.CaseId, tasks, cancellationToken);

        // response
        var response = new GetTaskListResponse();
        response.Tasks.AddRange(tasks);
        return response;
    }
}
