namespace NOBY.Api.Endpoints.Refinancing.GetMortgageExtraPayment;

internal sealed record GetMortgageExtraPaymentRequest(long CaseId, long ProcessId)
    : IRequest<RefinancingGetMortgageExtraPaymentResponse>
{
}
