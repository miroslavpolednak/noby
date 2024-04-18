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
                FeeAmountDiscounted = request.FeeAmountDiscounted
            },
            SimulationInputs = new()
            {
                ExtraPaymentAmount = request.ExtraPaymentAmount,
                ExtraPaymentDate = request.ExtraPaymentDate,
                ExtraPaymentReasonId = request.ExtraPaymentReasonId,
                IsExtraPaymentFullyRepaid = request.IsExtraPaymentFullyRepaid
            }
        };

        // spocitat simulaci
        var result = await _offerService.SimulateMortgageExtraPayment(dsRequest, cancellationToken);

        return new SimulateMortgageExtraPaymentResponse
        {
            OfferId = result.OfferId,
            ExtraPaymentAmount = result.SimulationResults.ExtraPaymentAmount,
            FeeAmount = result.SimulationResults.FeeAmount,
            InterestAmount = result.SimulationResults.InterestAmount,
            InterestCovid = result.SimulationResults.InterestCovid,
            InterestOnLate = result.SimulationResults.InterestOnLate,
            IsExtraPaymentComplete = result.SimulationResults.IsExtraPaymentFullyRepaid,
            NewMaturityDate = result.SimulationResults.NewMaturityDate,
            IsLoanOverdue = result.SimulationResults.IsLoanOverdue,
            IsPaymentReduced = result.SimulationResults.IsPaymentReduced,
            NewPaymentAmount = result.SimulationResults.NewPaymentAmount,
            OtherUnpaidFees = result.SimulationResults.OtherUnpaidFees,
            PrincipalAmount = result.SimulationResults.PrincipalAmount,
            
            FeeAmountTotal = result.SimulationResults.FeeAmount - request.FeeAmountDiscounted.GetValueOrDefault(),
            ExtraPaymentAmountTotal = result.SimulationResults.ExtraPaymentAmount + result.SimulationResults.FeeAmount - request.FeeAmountDiscounted.GetValueOrDefault()
        };
    }
}
