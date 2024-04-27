using DomainServices.OfferService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageExtraPayment;

internal sealed class SimulateMortgageExtraPaymentHandler(
    ISalesArrangementServiceClient _salesArrangementService,
    IOfferServiceClient _offerService)
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

        var response = new SimulateMortgageExtraPaymentResponse
        {
            OfferId = result.OfferId,
            NewOffer = result.SimulationResults.ToDto(request.FeeAmountDiscounted)
        };

        // najit puvodni simulaci
        var salesArrangements = await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellationToken);
        int? offerId = salesArrangements
            .SalesArrangements
            .FirstOrDefault(t => t.SalesArrangementTypeId == (int)SalesArrangementTypes.MortgageExtraPayment && t.OfferId.HasValue && t.State == (int)SalesArrangementStates.InApproval)
            ?.OfferId;

        if (offerId.HasValue)
        {
            var oldOffer = await _offerService.GetOffer(offerId.Value, cancellationToken);
            response.OldOffer = oldOffer.MortgageExtraPayment.SimulationResults.ToDto(oldOffer.MortgageExtraPayment.BasicParameters.FeeAmountDiscounted);
        }

        return response;
    }
}
