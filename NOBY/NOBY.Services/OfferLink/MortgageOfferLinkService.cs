using DomainServices.OfferService.Clients;
using DomainServices.OfferService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Services.OfferLink;

[ScopedService, SelfService]
public class MortgageOfferLinkService
{
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;

    public MortgageOfferLinkService(ISalesArrangementServiceClient salesArrangementService, IOfferServiceClient offerService)
    {
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
    }

    public async Task<(SalesArrangement salesArrangement, GetOfferResponse offer)> LoadAndValidateData(int salesArrangementId, int offerId, MortgageOfferLinkValidator validator, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellationToken);
        var offer = await _offerService.GetOffer(offerId, cancellationToken);

        await validator.ValidateAsync(salesArrangement, offer, cancellationToken);

        return (salesArrangement, offer);
    }

    public Task LinkOfferToSalesArrangement(int salesArrangementId, int offerId, CancellationToken cancellationToken) => 
        _salesArrangementService.LinkModelationToSalesArrangement(salesArrangementId, offerId, cancellationToken);
}