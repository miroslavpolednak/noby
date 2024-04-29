namespace NOBY.Api.Endpoints.Refinancing.GetMortgageExtraPaymentList;

internal sealed record GetMortgageExtraPaymentListRequest(long CaseId)
    : IRequest<GetMortgageExtraPaymentListResponse>
{
}
