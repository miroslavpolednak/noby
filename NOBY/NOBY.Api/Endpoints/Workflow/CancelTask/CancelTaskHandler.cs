using DomainServices.CaseService.Clients;

namespace NOBY.Api.Endpoints.Workflow.CancelTask;

internal sealed class CancelTaskHandler
    : IRequestHandler<CancelTaskRequest>
{
    public async Task Handle(CancelTaskRequest request, CancellationToken cancellationToken)
    {
        // jen check jestli case existuje
        await _caseService.ValidateCaseId(request.CaseId, true, cancellationToken);

        var task = await _caseService.GetTaskDetail(request.TaskIdSB, cancellationToken);
        if (!_allowedTypeIds.Contains(task.TaskObject?.TaskTypeId ?? 0) || task.TaskObject?.TaskIdSb == 30)
        {
            throw new CisAuthorizationException();
        }
        
        await _caseService.CancelTask(request.CaseId, request.TaskIdSB, cancellationToken);
    }

    private static int[] _allowedTypeIds = new[] { 2, 3 };

    private readonly ICaseServiceClient _caseService;

    public CancelTaskHandler(ICaseServiceClient caseService)
    {
        _caseService = caseService;
    }
}
