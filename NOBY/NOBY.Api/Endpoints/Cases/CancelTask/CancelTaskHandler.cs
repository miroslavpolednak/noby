using DomainServices.CaseService.Clients;

namespace NOBY.Api.Endpoints.Cases.CancelTask;

internal sealed class CancelTaskHandler
    : IRequestHandler<CancelTaskRequest>
{
    public async Task Handle(CancelTaskRequest request, CancellationToken cancellationToken)
    {
        await _caseService.CancelTask(request.TaskIdSB, cancellationToken);
    }

    private readonly ICaseServiceClient _caseService;

    public CancelTaskHandler(ICaseServiceClient caseService)
    {
        _caseService = caseService;
    }
}
