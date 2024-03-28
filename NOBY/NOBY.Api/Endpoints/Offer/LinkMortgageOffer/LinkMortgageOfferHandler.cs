using DomainServices.CaseService.Clients.v1;
using DomainServices.CaseService.Contracts;
using DomainServices.OfferService.Contracts;
using NOBY.Services.OfferLink;
using _SA = DomainServices.SalesArrangementService.Contracts.SalesArrangement;

namespace NOBY.Api.Endpoints.Offer.LinkMortgageOffer;

internal sealed class LinkMortgageOfferHandler : IRequestHandler<LinkMortgageOfferRequest>
{
    private static readonly MortgageOfferLinkValidator _validator = new()
    {
        SalesArrangementType = SalesArrangementTypes.Mortgage,
        OfferType = OfferTypes.Mortgage,
        AdditionalValidation = AdditionalValidation
    };

    private readonly ICaseServiceClient _caseService;
    private readonly MortgageOfferLinkService _mortgageOfferLinkService;

    public LinkMortgageOfferHandler(ICaseServiceClient caseService, MortgageOfferLinkService mortgageOfferLinkService)
    {
        _caseService = caseService;
        _mortgageOfferLinkService = mortgageOfferLinkService;
    }

    public async Task Handle(LinkMortgageOfferRequest request, CancellationToken cancellationToken)
    {
        var (salesArrangement, _) = await _mortgageOfferLinkService.LoadAndValidateData(request.SalesArrangementId, request.OfferId, _validator, cancellationToken);

        await UpdateOfferContracts(salesArrangement.CaseId, request.OfferContacts, cancellationToken);
        await UpdateCustomerData(salesArrangement.CaseId, request, cancellationToken);

        await _mortgageOfferLinkService.LinkOfferToSalesArrangement(request.SalesArrangementId, request.OfferId, cancellationToken);
    }

    private async Task UpdateOfferContracts(long caseId, Dto.ContactsDto? contacts, CancellationToken cancellationToken)
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

    private async Task UpdateCustomerData(long caseId, LinkMortgageOfferRequest request, CancellationToken cancellationToken)
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