using DomainServices.CaseService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Refinancing.GetMortgageExtraPaymentList;

internal sealed class GetMortgageExtraPaymentListHandler(
    ISalesArrangementServiceClient _salesArrangementService,
    ICaseServiceClient _caseService)
    : IRequestHandler<GetMortgageExtraPaymentListRequest, GetMortgageExtraPaymentListResponse>
{
    public async Task<GetMortgageExtraPaymentListResponse> Handle(GetMortgageExtraPaymentListRequest request, CancellationToken cancellationToken)
    {
        var saList = await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellationToken);
        var tasks = await _caseService.GetTaskList(request.CaseId, cancellationToken);

        throw new NotImplementedException();
    }
}
