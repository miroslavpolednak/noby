using DomainServices.OfferService.Clients.v1;
using NOBY.Services.MortgageRefinancing;

namespace NOBY.Api.Endpoints.Refinancing.GetMortgageExtraPayment;

internal sealed class GetMortgageExtraPaymentHandler(
    IOfferServiceClient _offerService,
    Services.ResponseCodes.ResponseCodesService _responseCodes,
    MortgageRefinancingWorkflowService _refinancingWorkflowService)
    : IRequestHandler<GetMortgageExtraPaymentRequest, GetMortgageExtraPaymentResponse>
{
    public async Task<GetMortgageExtraPaymentResponse> Handle(GetMortgageExtraPaymentRequest request, CancellationToken cancellationToken)
    {
        var extraPaymentData = await _refinancingWorkflowService.GetRefinancingData(request.CaseId, request.ProcessId, RefinancingTypes.MortgageExtraPayment, cancellationToken);

        throw new NotImplementedException();
    }
}
