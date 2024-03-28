using DomainServices.OfferService.Contracts;
using NOBY.Services.OfferLink;
using _SA = DomainServices.SalesArrangementService.Contracts.SalesArrangement;

namespace NOBY.Api.Endpoints.Offer.LinkMortgageExtraPayment;

internal class LinkMortgageExtraPaymentHandler : IRequestHandler<LinkMortgageExtraPaymentRequest>
{
    private static readonly MortgageOfferLinkValidator _validator = new()
    {
        SalesArrangementType = SalesArrangementTypes.MortgageExtraPayment,
        OfferType = OfferTypes.MortgageExtraPayment,
        AdditionalValidation = AdditionalValidation
    };

    private readonly MortgageOfferLinkService _mortgageOfferLinkService;

    public LinkMortgageExtraPaymentHandler(MortgageOfferLinkService mortgageOfferLinkService)
    {
        _mortgageOfferLinkService = mortgageOfferLinkService;
    }

    public async Task Handle(LinkMortgageExtraPaymentRequest request, CancellationToken cancellationToken)
    {
        await _mortgageOfferLinkService.LoadAndValidateData(request.SalesArrangementId, request.OfferId, _validator, cancellationToken);

        await _mortgageOfferLinkService.LinkOfferToSalesArrangement(request.SalesArrangementId, request.OfferId, cancellationToken);
    }

    private static Task<bool> AdditionalValidation(_SA salesArrangement, GetOfferResponse offer, CancellationToken cancellationToken)
    {
        return Task.FromResult(salesArrangement.CaseId == offer.Data.CaseId && (DateTime.UtcNow - offer.Data.Created.DateTime).TotalDays >= 1);
    }
}