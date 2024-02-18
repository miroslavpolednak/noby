﻿using CIS.Core.Security;
using DomainServices.SalesArrangementService.Clients;
using _Ca = DomainServices.CaseService.Contracts;

namespace NOBY.Api.Endpoints.Offer.LinkModelation;

internal sealed class LinkModelationHandler
    : IRequestHandler<LinkModelationRequest>
{
    public async Task Handle(LinkModelationRequest request, CancellationToken cancellationToken)
    {
        // get SA data
        var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        

        if (saInstance.SalesArrangementTypeId == (int)SalesArrangementTypes.Mortgage)
        {
            if (!_currentUserAccessor.HasPermission(UserPermissions.SALES_ARRANGEMENT_Access))
            {
                throw new CisAuthorizationException();
            }
            await updateMortgage(request, saInstance, cancellationToken);
        }
        else if (saInstance.SalesArrangementTypeId is (int)SalesArrangementTypes.Refixation or (int)SalesArrangementTypes.Retention)
        {
            if (!_currentUserAccessor.HasPermission(UserPermissions.SALES_ARRANGEMENT_RefinancingAccess))
            {
                throw new CisAuthorizationException();
            }

            //... implementace
        }
        else
        {
            throw new NobyValidationException(90032);
        }

        // nalinkovat novou simulaci
        await _salesArrangementService.LinkModelationToSalesArrangement(request.SalesArrangementId, request.OfferId, cancellationToken);
    }

    private async Task updateMortgage(LinkModelationRequest request, DomainServices.SalesArrangementService.Contracts.SalesArrangement saInstance, CancellationToken cancellationToken)
    {
        // get case instance
        var caseInstance = await _caseService.GetCaseDetail(saInstance.CaseId, cancellationToken);

        // update kontaktu
        var offerContacts = new _Ca.OfferContacts
        {
            EmailForOffer = request.OfferContacts?.EmailAddress?.EmailAddress ?? "",
            PhoneNumberForOffer = new _Ca.Phone
            {
                PhoneNumber = request.OfferContacts?.MobilePhone?.PhoneNumber ?? "",
                PhoneIDC = request.OfferContacts?.MobilePhone?.PhoneIDC ?? ""
            }
        };
        await _caseService.UpdateOfferContacts(saInstance.CaseId, offerContacts, cancellationToken);

        // update customer
        if (caseInstance.Customer?.Identity is null || caseInstance.Customer.Identity.IdentityId == 0)
        {
            await _caseService.UpdateCustomerData(saInstance.CaseId, new _Ca.CustomerData
            {
                DateOfBirthNaturalPerson = request.DateOfBirth,
                FirstNameNaturalPerson = request.FirstName ?? "",
                Name = request.LastName ?? "",
            }, cancellationToken);
        }
    }

    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly DomainServices.CaseService.Clients.ICaseServiceClient _caseService;

    public LinkModelationHandler(
        DomainServices.CaseService.Clients.ICaseServiceClient caseService,
        ISalesArrangementServiceClient salesArrangementService,
        ICurrentUserAccessor currentUserAccessor)
    {
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
        _currentUserAccessor = currentUserAccessor;
    }
}
