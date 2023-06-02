using DomainServices.CaseService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Workflow.GetCurrentPriceException;

internal sealed class GetCurrentPriceExceptionHandler
    : IRequestHandler<GetCurrentPriceExceptionRequest, GetCurrentPriceExceptionResponse>
{
    public async Task<GetCurrentPriceExceptionResponse> Handle(GetCurrentPriceExceptionRequest request, CancellationToken cancellationToken)
    {
        var taskList = await _caseService.GetTaskList(request.CaseId, cancellationToken);
        var priceException = taskList.FirstOrDefault(t => t.TaskTypeId == 2 && !t.Cancelled);

        if (priceException is not null)
        {
            var taskDetail = await _caseService.GetTaskDetail(priceException.TaskIdSb, cancellationToken);
        }

        return null;
    }

    private readonly ICaseServiceClient _caseService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public GetCurrentPriceExceptionHandler(ICaseServiceClient caseService, ISalesArrangementServiceClient salesArrangementService)
    {
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
    }
}
