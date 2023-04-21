using DomainServices.CaseService.Clients;

namespace NOBY.Api.Endpoints.Cases.CreateTask;

internal sealed class CreateTaskHandler
    : IRequestHandler<CreateTaskRequest, int>
{
    public async Task<int> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        // kontrola existence Case
        await _caseService.GetCaseDetail(request.CaseId, cancellationToken);

        var result = await _caseService.CreateTask(new DomainServices.CaseService.Contracts.CreateTaskRequest
        {
            ProcessId = request.ProcessId,
            TaskTypeId = request.TaskTypeId,
            TaskRequest = request.TaskUserRequest,
            TaskSubtypeId = request.TaskSubtypeId
        }, cancellationToken);

        return result.TaskId;
    }

    private readonly ICaseServiceClient _caseService;

    public CreateTaskHandler(ICaseServiceClient caseService)
    {
        _caseService = caseService;
    }
}
