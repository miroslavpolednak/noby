namespace NOBY.Api.Endpoints.Refinancing.GetExtraPaymentList;

internal sealed record GetExtraPaymentListRequest(long CaseId)
    : IRequest<GetExtraPaymentListResponse>
{
}
