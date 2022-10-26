using DomainServices.SalesArrangementService.Abstraction;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Offer.LinkModelation;

internal class LinkModelationHandler
    : AsyncRequestHandler<LinkModelationRequest>
{
    protected override async Task Handle(LinkModelationRequest request, CancellationToken cancellationToken)
    {
        var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken));

        // nalinkovat novou simulaci
        await _salesArrangementService.LinkModelationToSalesArrangement(request.SalesArrangementId, request.OfferId, cancellationToken);
        
        // update kontaktu
        await _caseService.UpdateOfferContacts(saInstance.CaseId, new DomainServices.CaseService.Contracts.OfferContacts
        {
            EmailForOffer = request.EmailForOffer ?? "",
            PhoneNumberForOffer = request.PhoneNumberForOffer ?? ""
        }, cancellationToken);
    }

    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;

    public LinkModelationHandler(
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService,
        ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
    }
}
