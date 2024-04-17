using DomainServices.OfferService.Clients.v1;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageExtraPayment;

internal sealed class SimulateMortgageExtraPaymentHandler(IOfferServiceClient _offerService)
    : IRequestHandler<SimulateMortgageExtraPaymentRequest, SimulateMortgageExtraPaymentResponse>
{
    public async Task<SimulateMortgageExtraPaymentResponse> Handle(SimulateMortgageExtraPaymentRequest request, CancellationToken cancellationToken)
    {
        var dsRequest = new DomainServices.OfferService.Contracts.SimulateMortgageExtraPaymentRequest
        {
            CaseId = request.CaseId,
            BasicParameters = new()
            {
                FeeAmountDiscount = 0
            },
            SimulationInputs = new()
            {
                ExtraPaymentAmount = request.ExtraPaymentAmount,
                ExtraPaymentDate = request.ExtraPaymentDate,
                ExtraPaymentReasonId = request.ExtraPaymentReasonId,
                IsExtraPaymentComplete = request.IsExtraPaymentFullyRepaid
            }
        };

        // spocitat simulaci
        var result = await _offerService.SimulateMortgageExtraPayment(dsRequest, cancellationToken);

        return new SimulateMortgageExtraPaymentResponse
        {
            OfferId = result.OfferId,
            ExtraPaymentAmount = result.SimulationResults.ExtraPaymentAmount,
            FeeAmount = result.SimulationResults.ExtraPaymentAmount,
            InterestAmount = result.SimulationResults.InterestAmount,
            InterestCovid = result.SimulationResults.InterestCovid,
            InterestOnLate = result.SimulationResults.InterestOnLate,
            IsExtraPaymentComplete = result.SimulationResults.IsExtraPaymentComplete,
            NewMaturityDate = result.SimulationResults.NewMaturityDate,
            IsLoanOverdue = result.SimulationResults.IsLoanOverdue,
            IsPaymentReduced = result.SimulationResults.IsPaymentReduced,
            NewPaymentAmount = result.SimulationResults.NewPaymentAmount,
            OtherUnpaidFees = result.SimulationResults.OtherUnpaidFees,
            PrincipalAmount = result.SimulationResults.PrincipalAmount
        };
    }
}
