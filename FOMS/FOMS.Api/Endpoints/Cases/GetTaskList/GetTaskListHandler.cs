namespace FOMS.Api.Endpoints.Cases.GetTaskList;

internal sealed class GetTaskListHandler
    : IRequestHandler<GetTaskListRequest, GetTaskListResponse>
{
    public async Task<GetTaskListResponse> Handle(GetTaskListRequest request, CancellationToken cancellationToken)
    {
        var result = ServiceCallResult.ResolveAndThrowIfError<DomainServices.CaseService.Contracts.GetTaskListResponse>(await _caseService.GetTaskList(request.CaseId, cancellationToken));

        return new GetTaskListResponse
        {
            Tasks = result.Tasks?.Select(t => new Dto.WorkflowTask
            {
                StateId = t.StateId,
                CreatedOn = t.CreatedOn,
                Name = t.Name,
                TaskId = t.TaskId,
                TaskProcessId = t.TaskProcessId,
                TypeId = t.TypeId
            }).ToList()
        };
    }

    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;

    public GetTaskListHandler(
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService)
    {
        _caseService = caseService;
    }
}
