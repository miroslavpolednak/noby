﻿using _HO = DomainServices.HouseholdService.Contracts;
using _Cust = DomainServices.CustomerService.Contracts;
using CIS.Infrastructure.gRPC.CisTypes;
using System.ComponentModel.DataAnnotations;
using CIS.Core;
using NOBY.Api.Extensions;
using CIS.Core.Security;

namespace NOBY.Api.Endpoints.Cases.GetCustomers;

internal sealed class GetCustomersHandler
    : IRequestHandler<GetCustomersRequest, List<GetCustomersResponseCustomer>>
{
    public async Task<List<GetCustomersResponseCustomer>> Handle(GetCustomersRequest request, CancellationToken cancellationToken)
    {
        // data o CASE-u
        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);

        // seznam zemi
        var countries = (await _codebookService.Countries(cancellationToken));

        List<(Identity? Identity, _HO.CustomerOnSA? CustomerOnSA, int Role, string RoleName, bool Agent, bool IsKYCSuccessful)> customerIdentities;

        if (caseInstance.State == (int)CIS.Foms.Enums.CaseStates.InProgress)
        {
            var saId = await _salesArrangementService.GetProductSalesArrangement(request.CaseId, cancellationToken);
            // z parameters nacist Agent
            var saDetail = await _salesArrangementService.GetSalesArrangement(saId.SalesArrangementId, cancellationToken);
            
            // vsichni customeri z CustomerOnSA
            var customers = await _customerOnSAService.GetCustomerList(saId.SalesArrangementId, cancellationToken);

            // vybrat a transformovat jen vlastnik, spoludluznik
            customerIdentities = customers
                .Where(t => _allowedCustomerRoles.Contains(t.CustomerRoleId))
                .Select(t => (
                    t.CustomerIdentifiers?.FirstOrDefault(x => x.IdentityScheme == Identity.Types.IdentitySchemes.Kb),
                    (_HO.CustomerOnSA?)t,
                    t.CustomerRoleId,
                    ((CIS.Foms.Enums.CustomerRoles)t.CustomerRoleId).GetAttribute<DisplayAttribute>()!.Name ?? "",
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
                    IsKYCSuccessful: t.IsKYCSuccessful
                 ))
                .ToList();
        }

        // detail customeru z customerService
        var identifiedCustomers = customerIdentities.Where(t => t.Identity is not null).ToList();
        var customerDetails = new List<_Cust.CustomerDetailResponse>();
        if (identifiedCustomers.Any())
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

            var permanentAddress = customer.Addresses?.FirstOrDefault(x => x.AddressTypeId == (int)CIS.Foms.Enums.AddressTypes.Permanent);
            var mailingAddress = customer.Addresses?.FirstOrDefault(x => x.AddressTypeId == (int)CIS.Foms.Enums.AddressTypes.Mailing);
            var country = countries.FirstOrDefault(x => x.Id == customer.NaturalPerson.CitizenshipCountriesId.FirstOrDefault());

            var model = new GetCustomersResponseCustomer
            {
                Agent = t.Agent,
                IsKYCSuccessful = t.IsKYCSuccessful,
                Contacts = new(),
                KBID = customer.Identities.FirstOrDefault(x => x.IdentityScheme == Identity.Types.IdentitySchemes.Kb)?.IdentityId.ToString(System.Globalization.CultureInfo.InvariantCulture),
                RoleName = t.RoleName,
                RoleId = t.Role,
                DateOfBirth = customer.NaturalPerson?.DateOfBirth,
                LastName = customer.NaturalPerson?.LastName,
                FirstName = customer.NaturalPerson?.FirstName,
                PermanentAddress = permanentAddress,
                ContactAddress = mailingAddress,
                IdentificationDocument = customer.IdentificationDocument?.ToResponseDto(),
                CitizenshipCountry = country is null ? null : new()
                {
                    Id = country.Id,
                    Name = country?.Name
                }
            };

            var email = customer.Contacts?.FirstOrDefault(x => x.ContactTypeId == (int)CIS.Foms.Enums.ContactTypes.Email)?.Email?.EmailAddress;
            if (!string.IsNullOrEmpty(email))
                model.Contacts.EmailAddress = new() { EmailAddress = email };

            var phone = customer.Contacts?.FirstOrDefault(x => x.ContactTypeId == (int)CIS.Foms.Enums.ContactTypes.Mobil)?.Mobile?.PhoneNumber;
            if (!string.IsNullOrEmpty(phone))
                model.Contacts.MobilePhone = new()
                {
                    PhoneNumber = phone,
                    PhoneIDC = customer.Contacts!.First(x => x.ContactTypeId == (int)CIS.Foms.Enums.ContactTypes.Mobil).Mobile.PhoneIDC
                };

            return model;
        }).ToList();

        return finalCustomerList.OrderBy(t => t.RoleId).ThenBy(t => t.LastName).ToList();
    }

    private static int[] _allowedCustomerRoles = new[] { 1, 2 };
    
    private readonly DomainServices.ProductService.Clients.IProductServiceClient _productService;
    private readonly DomainServices.CustomerService.Clients.ICustomerServiceClient _customerService;
    private readonly DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService;
    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient _customerOnSAService;
    private readonly DomainServices.CaseService.Clients.ICaseServiceClient _caseService;

    public GetCustomersHandler(
        DomainServices.ProductService.Clients.IProductServiceClient productService,
        DomainServices.CustomerService.Clients.ICustomerServiceClient customerService,
        DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient customerOnSAService,
        DomainServices.CodebookService.Clients.ICodebookServiceClient codebookService,
        DomainServices.CaseService.Clients.ICaseServiceClient caseService, 
        DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService)
    {
        _productService = productService;
        _customerService = customerService;
        _customerOnSAService = customerOnSAService;
        _codebookService = codebookService;
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
    }
}
