using _Case = DomainServices.CaseService.Contracts;
using _Offer = DomainServices.OfferService.Contracts;
using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.Offer.CreateMortgageCase;

internal static class Extensions
{
    public static _HO.CreateCustomerRequest ToDomainServiceRequest(this CreateMortgageCaseRequest request, int salesArrangementId)
    {
        var model = new _HO.CreateCustomerRequest
        {
            SalesArrangementId = salesArrangementId,
            CustomerRoleId = (int)SharedTypes.Enums.CustomerRoles.Debtor,
            Customer = new _HO.CustomerOnSABase
            {
                DateOfBirthNaturalPerson = request.DateOfBirth,
                FirstNameNaturalPerson = request.FirstName,
                Name = request.LastName
            }
        };
        if (request.Identity is not null && request.Identity.Id > 0)
            model.Customer.CustomerIdentifiers.Add(new SharedTypes.GrpcTypes.Identity(request.Identity!));

        return model;
    }
    
    /// <summary>
    /// Vytvoreni requestu pro zalozeni CASE
    /// </summary>
    public static _Case.CreateCaseRequest ToDomainServiceRequest(this CreateMortgageCaseRequest request, int userId, _Offer.MortgageOfferSimulationInputs offerInstance)
        => new _Case.CreateCaseRequest
        {
            CaseOwnerUserId = userId,
            Customer = new _Case.CustomerData
            {
                DateOfBirthNaturalPerson = request.DateOfBirth,
                FirstNameNaturalPerson = request.FirstName ?? "",
                Name = request.LastName ?? "",
                Identity = request.Identity is null ? null : new SharedTypes.GrpcTypes.Identity(request.Identity)
            },
            OfferContacts = new _Case.OfferContacts
            {
                EmailForOffer = request.OfferContacts?.EmailAddress?.EmailAddress ?? "",
                PhoneNumberForOffer = new()
                {
                    PhoneNumber = request.OfferContacts?.MobilePhone?.PhoneNumber ?? "",
                    PhoneIDC = request.OfferContacts?.MobilePhone?.PhoneIDC ?? ""
                }
            },
            Data = new _Case.CaseData
            {
                IsEmployeeBonusRequested = offerInstance.IsEmployeeBonusRequested,
                ProductTypeId = offerInstance.ProductTypeId,
                TargetAmount = (decimal)offerInstance.LoanAmount!
            }
        };
}
