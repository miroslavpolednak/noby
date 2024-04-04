using DomainServices.OfferService.Clients.v1;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageRefixationOfferList;

internal sealed class SimulateMortgageRefixationOfferListHandler
    : IRequestHandler<SimulateMortgageRefixationOfferListRequest, SimulateMortgageRefixationOfferListResponse>
{
    public async Task<SimulateMortgageRefixationOfferListResponse> Handle(SimulateMortgageRefixationOfferListRequest request, CancellationToken cancellationToken)
    {
        var offers = (await _offerService.GetOfferList(request.CaseId, DomainServices.OfferService.Contracts.OfferTypes.MortgageRefixation, false, cancellationToken))
            .Where(t => t.Data.ValidTo >= _timeProvider.GetLocalNow().Date)
            .ToList();

        List<Dto.Refinancing.RefinancingOfferDetail> finalOffers = new();

        foreach (var offer in offers)
        {
            // IC je rozdilna mezi ulozenou offer a requestem
            if (!((OfferFlagTypes)offer.Data.Flags).HasFlag(OfferFlagTypes.LegalNotice)
                && offer.MortgageRefixation.SimulationInputs.InterestRateDiscount != request.InterestRateDiscount)
            {
                var simulateRequest = new DomainServices.OfferService.Contracts.SimulateMortgageRefixationRequest
                {
                    CaseId = request.CaseId,
                    OfferId = offer.Data.OfferId,
                    IsVirtual = true,
                    SimulationInputs = offer.MortgageRefixation.SimulationInputs,
                    BasicParameters = offer.MortgageRefixation.BasicParameters
                };
                simulateRequest.SimulationInputs.InterestRateDiscount = request.InterestRateDiscount;

                var result = await _offerService.SimulateMortgageRefixation(simulateRequest, cancellationToken);
                
                var item = Dto.Refinancing.RefinancingOfferDetail.CreateRefixationOffer(offer);
                item.InterestRateDiscount = result.SimulationInputs.InterestRateDiscount;
                item.LoanPaymentAmountDiscounted = result.SimulationResults.LoanPaymentAmountDiscounted;

                finalOffers.Add(item);
            }
            else
            {
                finalOffers.Add(Dto.Refinancing.RefinancingOfferDetail.CreateRefixationOffer(offer));
            }
        }

        return new SimulateMortgageRefixationOfferListResponse
        {
            Offers = finalOffers
        };
    }

    private readonly TimeProvider _timeProvider;
    private readonly IOfferServiceClient _offerService;

    public SimulateMortgageRefixationOfferListHandler(IOfferServiceClient offerService, TimeProvider timeProvider)
    {
        _offerService = offerService;
        _timeProvider = timeProvider;
    }
}
