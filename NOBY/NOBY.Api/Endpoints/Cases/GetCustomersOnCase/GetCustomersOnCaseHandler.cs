using _HO = DomainServices.HouseholdService.Contracts;
using _Cust = DomainServices.CustomerService.Contracts;
using SharedTypes.GrpcTypes;
using System.ComponentModel.DataAnnotations;
using CIS.Core;
using NOBY.Services.Customer;

namespace NOBY.Api.Endpoints.Cases.GetCustomersOnCase;

internal sealed class GetCustomersOnCaseHandler(
    DomainServices.ProductService.Clients.IProductServiceClient _productService,
    DomainServices.CustomerService.Clients.ICustomerServiceClient _customerService,
    DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient _customerOnSAService,
    DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService,
    DomainServices.CaseService.Clients.v1.ICaseServiceClient _caseService,
    DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService)
        : IRequestHandler<GetCustomersOnCaseRequest, List<CasesGetCustomersResponseCustomer>>
{
    public async Task<List<CasesGetCustomersResponseCustomer>> Handle(GetCustomersOnCaseRequest request, CancellationToken cancellationToken)
    {
        // data o CASE-u
        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);

        // seznam zemi
        var countries = (await _codebookService.Countries(cancellationToken));

        List<(Identity? Identity, _HO.CustomerOnSA? CustomerOnSA, int Role, string RoleName, bool Agent, bool IsKYCSuccessful)> customerIdentities;

        if (caseInstance.State == (int)SharedTypes.Enums.CaseStates.InProgress)
        {
            var saId = (await _salesArrangementService.GetProductSalesArrangements(request.CaseId, cancellationToken)).First().SalesArrangementId;
            // z parameters nacist Agent
            var saDetail = await _salesArrangementService.GetSalesArrangement(saId, cancellationToken);
            
            // vsichni customeri z CustomerOnSA
            var customers = await _customerOnSAService.GetCustomerList(saId, cancellationToken);

            // vybrat a transformovat jen vlastnik, spoludluznik
            customerIdentities = customers
                .Where(t => _allowedCustomerRoles.Contains(t.CustomerRoleId))
                .Select(t => (
                    t.CustomerIdentifiers?.FirstOrDefault(x => x.IdentityScheme == Identity.Types.IdentitySchemes.Kb),
                    (_HO.CustomerOnSA?)t,
                    t.CustomerRoleId,
                    ((SharedTypes.Enums.CustomerRoles)t.CustomerRoleId).GetAttribute<DisplayAttribute>()!.Name ?? "",
                    saDetail.Mortgage?.Agent.GetValueOrDefault() == t.CustomerOnSAId,
                    false
                ))
                .ToList();
        }
        else
        {
            // vsichni customeri v KonsDB
            var customers = await _productService.GetCustomersOnProduct(request.CaseId, cancellationToken);
            var roles = await _codebookService.RelationshipCustomerProductTypes(cancellationToken);

            // vybrat a transformovat jen vlastnik, spoludluznik
            customerIdentities = customers
                .Customers
                .Where(t => _allowedCustomerRoles.Contains(t.RelationshipCustomerProductTypeId))
                .Select(t => (
                    Identity: t.CustomerIdentifiers.FirstOrDefault(x => x.IdentityScheme == Identity.Types.IdentitySchemes.Kb),
                    CustomerOnSA: default(_HO.CustomerOnSA),
                    Role: t.RelationshipCustomerProductTypeId,
                    RoleName: roles.FirstOrDefault(x => x.Id == t.RelationshipCustomerProductTypeId)?.NameNoby ?? "",
                    Agent: t.Agent ?? false,
                    t.IsKYCSuccessful
                 ))
                .ToList();
        }

        // detail customeru z customerService
        var identifiedCustomers = customerIdentities.Where(t => t.Identity is not null).ToList();
        var customerDetails = new List<_Cust.CustomerDetailResponse>();
        if (identifiedCustomers.Count != 0)
        {
            customerDetails = (await _customerService.GetCustomerList(identifiedCustomers.Select(t => t.Identity!), cancellationToken)).Customers.ToList();
        }

        var finalCustomerList = customerIdentities.Select(t =>
        {
            var customer = t.Identity is null ? new _Cust.CustomerDetailResponse
            {
                NaturalPerson = new _Cust.NaturalPerson
                {
                    FirstName = t.CustomerOnSA!.FirstNameNaturalPerson,
                    LastName = t.CustomerOnSA.Name,
                    DateOfBirth = t.CustomerOnSA.DateOfBirthNaturalPerson
                }
            } : customerDetails.First(x => x.Identities.Any(i => i.IdentityId == t.Identity.IdentityId && i.IdentityScheme == t.Identity.IdentityScheme));

            var permanentAddress = customer.Addresses?.FirstOrDefault(x => x.AddressTypeId == (int)SharedTypes.Enums.AddressTypes.Permanent);
            var mailingAddress = customer.Addresses?.FirstOrDefault(x => x.AddressTypeId == (int)SharedTypes.Enums.AddressTypes.Mailing);
            var country = countries.FirstOrDefault(x => x.Id == customer.NaturalPerson.CitizenshipCountriesId.FirstOrDefault());

            var model = new CasesGetCustomersResponseCustomer
            {
                Agent = t.Agent,
                IsKYCSuccessful = t.IsKYCSuccessful,
                Contacts = new(),
                KbId = customer.Identities.FirstOrDefault(x => x.IdentityScheme == Identity.Types.IdentitySchemes.Kb)?.IdentityId.ToString(System.Globalization.CultureInfo.InvariantCulture),
                RoleName = t.RoleName,
                RoleId = t.Role,
                DateOfBirth = customer.NaturalPerson?.DateOfBirth,
                LastName = customer.NaturalPerson?.LastName,
                FirstName = customer.NaturalPerson?.FirstName,
                PermanentAddress = permanentAddress,
                ContactAddress = mailingAddress,
                IdentificationDocument = customer.IdentificationDocument?.ToFeApi(),
                CitizenshipCountry = country is null ? null : new()
                {
                    Id = country.Id,
                    Name = country?.Name
                }
            };

            var email = customer.Contacts?.FirstOrDefault(x => x.ContactTypeId == (int)SharedTypes.Enums.ContactTypes.Email)?.Email?.EmailAddress;
            if (!string.IsNullOrEmpty(email))
                model.Contacts.EmailAddress = new() { EmailAddress = email };

            var phone = customer.Contacts?.FirstOrDefault(x => x.ContactTypeId == (int)SharedTypes.Enums.ContactTypes.Mobil)?.Mobile?.PhoneNumber;
            if (!string.IsNullOrEmpty(phone))
                model.Contacts.MobilePhone = new()
                {
                    PhoneNumber = phone,
                    PhoneIDC = customer.Contacts!.First(x => x.ContactTypeId == (int)SharedTypes.Enums.ContactTypes.Mobil).Mobile.PhoneIDC
                };

            return model;
        }).ToList();

        return finalCustomerList.OrderBy(t => t.RoleId).ThenBy(t => t.LastName).ToList();
    }

    private static readonly int[] _allowedCustomerRoles = [1, 2];
}
