using _Case = DomainServices.CaseService.Contracts;
using _Offer = DomainServices.OfferService.Contracts;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Offer.CreateMortgageCase;

internal static class Extensions
{
    public static _SA.CreateCustomerRequest ToDomainServiceRequest(this CreateMortgageCaseRequest request, int salesArrangementId)
    {
        var model = new _SA.CreateCustomerRequest
        {
            SalesArrangementId = salesArrangementId,
            CustomerRoleId = (int)CIS.Foms.Enums.CustomerRoles.Debtor,
            Customer = new _SA.CustomerOnSABase
            {
                DateOfBirthNaturalPerson = request.DateOfBirth,
                FirstNameNaturalPerson = request.FirstName,
                Name = request.LastName
            }
        };
        if (request.Identity is not null && request.Identity.Id > 0)
            model.Customer.CustomerIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.Identity(request.Identity!));

        return model;
    }
    
    /// <summary>
    /// Vytvoreni requestu pro zalozeni CASE
    /// </summary>
    public static _Case.CreateCaseRequest ToDomainServiceRequest(this CreateMortgageCaseRequest request, int userId, _Offer.MortgageSimulationInputs offerInstance)
        => new _Case.CreateCaseRequest
        {
            CaseOwnerUserId = userId,
            Customer = new _Case.CustomerData
            {
                DateOfBirthNaturalPerson = request.DateOfBirth,
                FirstNameNaturalPerson = request.FirstName,
                Name = request.LastName,
                Identity = request.Identity is null ? null : new CIS.Infrastructure.gRPC.CisTypes.Identity(request.Identity)
            },
            Data = new _Case.CaseData
            {
                ProductTypeId = offerInstance.ProductTypeId,
                TargetAmount = (decimal)offerInstance.LoanAmount!
            }
        };
}
