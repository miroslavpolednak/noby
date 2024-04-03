using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Services.OfferLink;
using _SA = DomainServices.SalesArrangementService.Contracts.SalesArrangement;

namespace NOBY.Api.Endpoints.Offer.LinkMortgageExtraPayment;

internal class LinkMortgageExtraPaymentHandler : IRequestHandler<LinkMortgageExtraPaymentRequest>
{
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;

    private static readonly MortgageOfferLinkValidator _validator = new()
    {
        SalesArrangementType = SalesArrangementTypes.MortgageExtraPayment,
        OfferType = OfferTypes.MortgageExtraPayment,
        AdditionalValidation = AdditionalValidation
    };

    public LinkMortgageExtraPaymentHandler(ISalesArrangementServiceClient salesArrangementService, IOfferServiceClient offerService)
    {
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
    }

    public async Task Handle(LinkMortgageExtraPaymentRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        var offer = await _offerService.GetOffer(request.OfferId, cancellationToken);

        await _validator.Validate(salesArrangement, offer, cancellationToken);

        await _salesArrangementService.LinkModelationToSalesArrangement(request.SalesArrangementId, request.OfferId, cancellationToken);
    }

    private static Task<bool> AdditionalValidation(_SA salesArrangement, GetOfferResponse offer, CancellationToken cancellationToken)
    {
        return Task.FromResult(salesArrangement.CaseId == offer.Data.CaseId && (DateTime.UtcNow - offer.Data.Created.DateTime).TotalDays >= 1);
    }
}