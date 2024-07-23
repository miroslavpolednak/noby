using DomainServices.OfferService.Clients.v1;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageRefixationOfferList;

internal sealed class SimulateMortgageRefixationOfferListHandler(
    IOfferServiceClient _offerService, 
    TimeProvider _timeProvider)
        : IRequestHandler<OfferSimulateMortgageRefixationOfferListRequest, OfferSimulateMortgageRefixationOfferListResponse>
{
    public async Task<OfferSimulateMortgageRefixationOfferListResponse> Handle(OfferSimulateMortgageRefixationOfferListRequest request, CancellationToken cancellationToken)
    {
        decimal? interestRateDiscount = request.InterestRateDiscount == 0 ? null : request.InterestRateDiscount;

        var offers = (await _offerService.GetOfferList(request.CaseId, DomainServices.OfferService.Contracts.OfferTypes.MortgageRefixation, false, cancellationToken))
            .Where(t => !(t.Data.ValidTo < _timeProvider.GetLocalNow().Date))
            .ToList();

        List<RefinancingSharedOfferDetail> finalOffers = [];

        foreach (var offer in offers)
        {
            // IC je rozdilna mezi ulozenou offer a requestem
            if (!((EnumOfferFlagTypes)offer.Data.Flags).HasFlag(EnumOfferFlagTypes.LegalNotice)
                && offer.MortgageRefixation.SimulationInputs.InterestRateDiscount != interestRateDiscount)
            {
                var simulateRequest = new DomainServices.OfferService.Contracts.SimulateMortgageRefixationRequest
                {
                    CaseId = request.CaseId,
                    OfferId = offer.Data.OfferId,
                    IsVirtual = true,
                    SimulationInputs = offer.MortgageRefixation.SimulationInputs,
                    BasicParameters = offer.MortgageRefixation.BasicParameters
                };
                simulateRequest.SimulationInputs.InterestRateDiscount = interestRateDiscount;

                var result = await _offerService.SimulateMortgageRefixation(simulateRequest, cancellationToken);
                
                var item = RefinancingSharedOfferDetail.CreateRefixationOffer(offer);
                item.InterestRateDiscount = result.SimulationInputs.InterestRateDiscount;
                item.LoanPaymentAmountDiscounted = result.SimulationResults.LoanPaymentAmountDiscounted;

                finalOffers.Add(item);
            }
            else
            {
                finalOffers.Add(RefinancingSharedOfferDetail.CreateRefixationOffer(offer));
            }
        }

        // validace rate
        if (request.InterestRateDiscount > 0 && (finalOffers.Min(t => t.InterestRate) - request.InterestRateDiscount) < 0.1M)
        {
            throw new NobyValidationException(90060);
        }

        return new()
        {
            Offers = finalOffers.OrderBy(t => t.FixedRatePeriod).ToList()
        };
    }
}
