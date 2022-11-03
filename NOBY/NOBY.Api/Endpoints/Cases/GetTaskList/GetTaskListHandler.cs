namespace NOBY.Api.Endpoints.Cases.GetTaskList;

internal sealed class GetTaskListHandler
    : IRequestHandler<GetTaskListRequest, GetTaskListResponse>
{
    public async Task<GetTaskListResponse> Handle(GetTaskListRequest request, CancellationToken cancellationToken)
    {
        var result = ServiceCallResult.ResolveAndThrowIfError<DomainServices.CaseService.Contracts.GetTaskListResponse>(await _caseService.GetTaskList(request.CaseId, cancellationToken));

        var taskTypes = await _codebookService.WorkflowTaskTypes(cancellationToken);

        return new GetTaskListResponse
        {
            Tasks = result.Tasks?.Select(t => new Dto.WorkflowTask
            {
                StateId = t.StateId,
                CreatedOn = t.CreatedOn,
                Name = t.Name,
                CategoryId = taskTypes.FirstOrDefault(x => x.Id == t.TypeId)?.CategoryId ?? 0,
                TaskId = t.TaskId,
                TaskProcessId = t.TaskProcessId,
                TypeId = t.TypeId
            }).ToList()
        };
    }

    private readonly DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly DomainServices.CaseService.Clients.ICaseServiceClient _caseService;

    public GetTaskListHandler(
        DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        DomainServices.CaseService.Clients.ICaseServiceClient caseService)
    {
        _codebookService = codebookService;
        _caseService = caseService;
    }
}
