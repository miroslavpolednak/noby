using DomainServices.CaseService.Clients.v1;
using DomainServices.CaseService.Contracts;
using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Services.OfferLink;
using _SA = DomainServices.SalesArrangementService.Contracts.SalesArrangement;

namespace NOBY.Api.Endpoints.Offer.LinkMortgageOffer;

internal sealed class LinkMortgageOfferHandler(
    ICaseServiceClient _caseService, 
    ISalesArrangementServiceClient _salesArrangementService, 
    IOfferServiceClient _offerService) 
    : IRequestHandler<OfferLinkMortgageOfferRequest, OfferRefinancingLinkResult>
{
    private static readonly MortgageOfferLinkValidator _validator = new()
    {
        SalesArrangementType = SalesArrangementTypes.Mortgage,
        OfferType = OfferTypes.Mortgage,
        AdditionalValidation = AdditionalValidation,
        ValidStates = { SharedTypes.Enums.EnumSalesArrangementStates.InSigning, SharedTypes.Enums.EnumSalesArrangementStates.ToSend }
    };

	public async Task<OfferRefinancingLinkResult> Handle(OfferLinkMortgageOfferRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        var offer = await _offerService.GetOffer(request.OfferId, cancellationToken);

        await _validator.Validate(salesArrangement, offer, cancellationToken);

        await UpdateOfferContracts(salesArrangement.CaseId, request.OfferContacts, cancellationToken);
        await UpdateCustomerData(salesArrangement.CaseId, request, cancellationToken);

        await _salesArrangementService.LinkModelationToSalesArrangement(request.SalesArrangementId, request.OfferId, cancellationToken);

        return new()
        {
            SalesArrangementId = salesArrangement.SalesArrangementId
        };
    }

    private async Task UpdateOfferContracts(long caseId, SharedTypesContacts? contacts, CancellationToken cancellationToken)
    {
        var offerContacts = new OfferContacts
        {
            EmailForOffer = contacts?.EmailAddress?.EmailAddress ?? "",
            PhoneNumberForOffer = new Phone
            {
                PhoneNumber = contacts?.MobilePhone?.PhoneNumber ?? "",
                PhoneIDC = contacts?.MobilePhone?.PhoneIDC ?? ""
            }
        };

        await _caseService.UpdateOfferContacts(caseId, offerContacts, cancellationToken);
    }

    private async Task UpdateCustomerData(long caseId, OfferLinkMortgageOfferRequest request, CancellationToken cancellationToken)
    {
        var caseInstance = await _caseService.GetCaseDetail(caseId, cancellationToken);

        if (caseInstance.Customer?.Identity is not null && caseInstance.Customer.Identity.IdentityId != 0)
            return;

        var customerData = new CustomerData
        {
            DateOfBirthNaturalPerson = request.DateOfBirth,
            FirstNameNaturalPerson = request.FirstName ?? "",
            Name = request.LastName ?? "",
        };

        await _caseService.UpdateCustomerData(caseId, customerData, cancellationToken);
    }

    private static Task<bool> AdditionalValidation(_SA salesArrangement, GetOfferResponse offer, CancellationToken cancellationToken) => 
        Task.FromResult(!offer.Data.CaseId.HasValue || salesArrangement.CaseId == offer.Data.CaseId);
}