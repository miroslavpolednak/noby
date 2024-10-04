using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;
using System.Globalization;

namespace NOBY.Api.Endpoints.Refinancing.CommunicateMortgageRefixation;

internal sealed class CommunicateMortgageRefixationHandler(
    ApiServices.MortgageRefinancingSalesArrangementCreateService _salesArrangementService,
    IOfferServiceClient _offerService)
    : IRequestHandler<CommunicateMortgageRefixationRequest, RefinancingSharedOfferLinkResult>
{
    public async Task<RefinancingSharedOfferLinkResult> Handle(CommunicateMortgageRefixationRequest request, CancellationToken cancellationToken)
    {
        // ziskat existujici nebo zalozit novy SA
        var sa = await _salesArrangementService.GetOrCreateSalesArrangement(request.CaseId, SalesArrangementTypes.MortgageRefixation, cancellationToken);

        var offerList = await _offerService.GetOfferList(request.CaseId, OfferTypes.MortgageRefixation, includeValidOnly: true, cancellationToken: cancellationToken);

        var currentOffers = offerList.Where(o => ((EnumOfferFlagTypes)o.Data.Flags).HasFlag(EnumOfferFlagTypes.Current)).ToList();
        var communicatedOffers = offerList.Where(o => ((EnumOfferFlagTypes)o.Data.Flags).HasFlag(EnumOfferFlagTypes.Communicated)).ToList();

        await _offerService.DeleteOfferList(communicatedOffers.Except(currentOffers).Select(o => o.Data.OfferId), cancellationToken);

        foreach (var offer in currentOffers)
        {
            var updateOfferRequest = new UpdateOfferRequest
            {
                OfferId = offer.Data.OfferId,
                ValidTo = new[] { DateTime.UtcNow.AddDays(45), (DateTime)offer.MortgageRefixation.BasicParameters.FixedRateValidTo }.Min(),
                Flags = (int)EnumOfferFlagTypes.Current | (int)EnumOfferFlagTypes.Communicated
            };

            if (offer.Data.Origin is not OfferOrigins.BigDataPlatform)
            {
                await CreateResponseCode(offer, cancellationToken);
            }

            await _offerService.UpdateOffer(updateOfferRequest, cancellationToken);
        }

        return new()
        {
            SalesArrangementId = sa.SalesArrangementId,
            ProcessId = sa.ProcessId
        };
    }

    private async Task CreateResponseCode(GetOfferListResponse.Types.GetOfferListItem offer, CancellationToken cancellationToken)
    {
        var serviceRequest = new CreateResponseCodeRequest
        {
            CaseId = offer.Data.CaseId ?? 0,
            ResponseCodeCategory = ResponseCodeCategories.NewFixedRatePeriod,
            Data = offer.MortgageRefixation.SimulationInputs.FixedRatePeriod.ToString(CultureInfo.InvariantCulture),
            ValidTo = DateTime.Now.AddYears(3) //??? co sem za platnost?
        };

        await _offerService.CreateResponseCode(serviceRequest, cancellationToken);
    }
}