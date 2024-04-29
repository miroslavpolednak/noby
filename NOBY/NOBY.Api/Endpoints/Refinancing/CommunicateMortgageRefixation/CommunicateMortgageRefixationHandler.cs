using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using System.Globalization;

namespace NOBY.Api.Endpoints.Refinancing.CommunicateMortgageRefixation;

internal sealed class CommunicateMortgageRefixationHandler : IRequestHandler<CommunicateMortgageRefixationRequest, CommunicateMortgageRefixationResponse>
{
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;

    public CommunicateMortgageRefixationHandler(ISalesArrangementServiceClient salesArrangementService, IOfferServiceClient offerService)
    {
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
    }

    public async Task<CommunicateMortgageRefixationResponse> Handle(CommunicateMortgageRefixationRequest request, CancellationToken cancellationToken)
    {
        var offerList = await _offerService.GetOfferList(request.CaseId, OfferTypes.MortgageRefixation, cancellationToken: cancellationToken);

        var currentOffers = offerList.Where(o => ((OfferFlagTypes)o.Data.Flags).HasFlag(OfferFlagTypes.Current)).ToList();
        var communicatedOffers = offerList.Where(o => ((OfferFlagTypes)o.Data.Flags).HasFlag(OfferFlagTypes.Communicated)).ToList();

        await _offerService.DeleteOfferList(communicatedOffers.Except(currentOffers).Select(o => o.Data.OfferId), cancellationToken);

        foreach (var offer in currentOffers)
        {
            var updateOfferRequest = new UpdateOfferRequest
            {
                OfferId = offer.Data.OfferId,
                ValidTo = new[] { DateTime.UtcNow.AddDays(45), (DateTime)offer.MortgageRefixation.BasicParameters.FixedRateValidTo }.Min(),
                Flags = (offer.Data.Flags & (int)OfferFlagTypes.Communicated) == 0 ? offer.Data.Flags | (int)OfferFlagTypes.Communicated : null
            };

            if (offer.Data.Origin is not OfferOrigins.BigDataPlatform)
            {
                await CreateResponseCode(offer, cancellationToken);
            }

            await _offerService.UpdateOffer(updateOfferRequest, cancellationToken);
        }

        var saList = await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellationToken);
        var sa = saList.SalesArrangements.FirstOrDefault(sa => sa.SalesArrangementTypeId == (int)SalesArrangementTypes.MortgageRefixation &&
                                                               sa.State is (int)SalesArrangementStates.NewArrangement or (int)SalesArrangementStates.InProgress);

        return new CommunicateMortgageRefixationResponse
        {
            ProcessId = sa?.ProcessId
        };
    }

    private async Task CreateResponseCode(GetOfferListResponse.Types.GetOfferListItem offer, CancellationToken cancellationToken)
    {
        var serviceRequest = new CreateResponseCodeRequest
        {
            CaseId = offer.Data.CaseId ?? 0,
            ResponseCodeCategory = ResponseCodeCategories.NewFixedRatePeriod,
            Data = offer.MortgageRefixation.SimulationInputs.FixedRatePeriod.ToString(CultureInfo.InvariantCulture),
        };

        await _offerService.CreateResponseCode(serviceRequest, cancellationToken);
    }
}