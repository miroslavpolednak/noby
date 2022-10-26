using DomainServices.SalesArrangementService.Abstraction;
using _Ca = DomainServices.CaseService.Contracts;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Offer.LinkModelation;

internal class LinkModelationHandler
    : AsyncRequestHandler<LinkModelationRequest>
{
    protected override async Task Handle(LinkModelationRequest request, CancellationToken cancellationToken)
    {
        // get SA data
        var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken));
        // get case instance
        var caseInstance = ServiceCallResult.ResolveAndThrowIfError<_Ca.Case>(await _caseService.GetCaseDetail(saInstance.CaseId, cancellationToken));

        // nalinkovat novou simulaci
        await _salesArrangementService.LinkModelationToSalesArrangement(request.SalesArrangementId, request.OfferId, cancellationToken);

        // update kontaktu
        await _caseService.UpdateOfferContacts(saInstance.CaseId, new DomainServices.CaseService.Contracts.OfferContacts
        {
            EmailForOffer = request.EmailForOffer ?? "",
            PhoneNumberForOffer = request.PhoneNumberForOffer ?? ""
        }, cancellationToken);

        // update customer
        if (caseInstance.Customer?.Identity is null || caseInstance.Customer.Identity.IdentityId == 0)
        {
            await _caseService.UpdateCaseCustomer(saInstance.CaseId, new DomainServices.CaseService.Contracts.CustomerData
            {
                DateOfBirthNaturalPerson = request.DateOfBirth,
                FirstNameNaturalPerson = request.FirstName,
                Name = request.LastName
            }, cancellationToken);
        }
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
