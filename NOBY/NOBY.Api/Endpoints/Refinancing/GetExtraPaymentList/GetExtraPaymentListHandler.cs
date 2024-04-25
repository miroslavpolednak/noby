using DomainServices.CaseService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Refinancing.GetExtraPaymentList;

internal sealed class GetExtraPaymentListHandler(
    ISalesArrangementServiceClient _salesArrangementService,
    ICaseServiceClient _caseService)
    : IRequestHandler<GetExtraPaymentListRequest, GetExtraPaymentListResponse>
{
    public async Task<GetExtraPaymentListResponse> Handle(GetExtraPaymentListRequest request, CancellationToken cancellationToken)
    {
        var saList = await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellationToken);
        var tasks = await _caseService.GetTaskList(request.CaseId, cancellationToken);

        throw new NotImplementedException();
    }
}
