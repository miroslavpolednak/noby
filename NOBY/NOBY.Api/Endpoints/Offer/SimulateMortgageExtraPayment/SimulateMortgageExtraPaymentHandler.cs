﻿using DomainServices.OfferService.Clients.v1;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageExtraPayment;

internal sealed class SimulateMortgageExtraPaymentHandler(IOfferServiceClient _offerService)
    : IRequestHandler<OfferSimulateMortgageExtraPaymentRequest, OfferSimulateMortgageExtraPaymentResponse>
{
    public async Task<OfferSimulateMortgageExtraPaymentResponse> Handle(OfferSimulateMortgageExtraPaymentRequest request, CancellationToken cancellationToken)
    {
        var dsRequest = new DomainServices.OfferService.Contracts.SimulateMortgageExtraPaymentRequest
        {
            CaseId = request.CaseId,
            BasicParameters = new()
            {
                FeeAmountDiscount = request.FeeAmountDiscount
            },
            SimulationInputs = new()
            {
                ExtraPaymentAmount = request.IsExtraPaymentFullyRepaid ? null : request.ExtraPaymentAmount,
                ExtraPaymentDate = request.ExtraPaymentDate,
                ExtraPaymentReasonId = request.ExtraPaymentReasonId,
                IsExtraPaymentFullyRepaid = request.IsExtraPaymentFullyRepaid
            }
        };

        // spocitat simulaci
        var result = await _offerService.SimulateMortgageExtraPayment(dsRequest, cancellationToken);

        return new()
        {
            OfferId = result.OfferId,
			SimulationResults = result.SimulationResults.ToDto(DateTime.Now, request.FeeAmountDiscount)
        };
    }
}
