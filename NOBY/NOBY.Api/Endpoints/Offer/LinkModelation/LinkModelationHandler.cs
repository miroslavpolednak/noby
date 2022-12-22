using DomainServices.SalesArrangementService.Clients;
using _Ca = DomainServices.CaseService.Contracts;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Offer.LinkModelation;

internal class LinkModelationHandler
    : AsyncRequestHandler<LinkModelationRequest>
{
    protected override async Task Handle(LinkModelationRequest request, CancellationToken cancellationToken)
    {
        // get SA data
        var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        // get case instance
        var caseInstance = await _caseService.GetCaseDetail(saInstance.CaseId, cancellationToken);

        // nalinkovat novou simulaci
        await _salesArrangementService.LinkModelationToSalesArrangement(request.SalesArrangementId, request.OfferId, cancellationToken);

        // update kontaktu
        await _caseService.UpdateOfferContacts(saInstance.CaseId, new _Ca.OfferContacts
        {
            EmailForOffer = request.EmailForOffer ?? "",
            PhoneNumberForOffer = request.PhoneNumberForOffer ?? ""
        }, cancellationToken);

        // update customer
        if (caseInstance.Customer?.Identity is null || caseInstance.Customer.Identity.IdentityId == 0)
        {
            await _caseService.UpdateCaseCustomer(saInstance.CaseId, new _Ca.CustomerData
            {
                DateOfBirthNaturalPerson = request.DateOfBirth,
                FirstNameNaturalPerson = request.FirstName ?? "",
                Name = request.LastName ?? ""
            }, cancellationToken);
        }
    }

    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly DomainServices.CaseService.Clients.ICaseServiceClient _caseService;

    public LinkModelationHandler(
        DomainServices.CaseService.Clients.ICaseServiceClient caseService,
        ISalesArrangementServiceClient salesArrangementService)
    {
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
    }
}
