﻿using _Case = DomainServices.CaseService.Contracts;
using _SA = DomainServices.SalesArrangementService.Contracts;
using _HO = DomainServices.HouseholdService.Contracts;
using _Cust = DomainServices.CustomerService.Contracts;
using CIS.Infrastructure.gRPC.CisTypes;
using System.ComponentModel.DataAnnotations;
using CIS.Core;

namespace NOBY.Api.Endpoints.Cases.GetCustomers;

internal class GetCustomersHandler
    : IRequestHandler<GetCustomersRequest, List<GetCustomersResponseCustomer>>
{
    public async Task<List<GetCustomersResponseCustomer>> Handle(GetCustomersRequest request, CancellationToken cancellationToken)
    {
        // data o CASE-u
        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);
        // seznam zemi
        var countries = (await _codebookService.Countries(cancellationToken));

        List<(Identity? Identity, _HO.CustomerOnSA? CustomerOnSA, int Role, bool Agent)> customerIdentities;

        if (caseInstance.State == (int)CIS.Foms.Enums.CaseStates.InProgress)
        {
            // get allowed SA types
            if (_allowedSalesArrangementTypes is null)
                _allowedSalesArrangementTypes = (await _codebookService.SalesArrangementTypes(cancellationToken))
                    .Where(t => t.ProductTypeId.GetValueOrDefault() > 0).Select(t => t.Id)
                    .ToList();

            // get salesArrangementId
            var saInstances = ServiceCallResult.ResolveAndThrowIfError<_SA.GetSalesArrangementListResponse>(
                await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellationToken: cancellationToken)
            );
            var saId = saInstances.SalesArrangements.First(t => _allowedSalesArrangementTypes.Contains(t.SalesArrangementTypeId)).SalesArrangementId;
            // z parameters nacist Agent
            var saDetail = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(saId, cancellationToken));
            
            // vsichni customeri z CustomerOnSA
            var customers = await _customerOnSAService.GetCustomerList(saId, cancellationToken);

            // vybrat a transformovat jen vlastnik, spoludluznik
            customerIdentities = customers
                .Where(t => _allowedCustomerRoles.Contains(t.CustomerRoleId))
                .Select(t => (
                    t.CustomerIdentifiers?.FirstOrDefault(x => x.IdentityScheme == Identity.Types.IdentitySchemes.Kb),
                    (_HO.CustomerOnSA?)t,
                    t.CustomerRoleId,
                    saDetail.Mortgage?.Agent.GetValueOrDefault() == t.CustomerOnSAId
                ))
                .ToList();
        }
        else
        {
            // vsichni customeri v KonsDB
            var customers = ServiceCallResult.ResolveAndThrowIfError<DomainServices.ProductService.Contracts.GetCustomersOnProductResponse>(await _productService.GetCustomersOnProduct(request.CaseId, cancellationToken));

            // vybrat a transformovat jen vlastnik, spoludluznik
            customerIdentities = customers
                .Customers
                .Where(t => _allowedCustomerRoles.Contains(t.RelationshipCustomerProductTypeId))
                .Select(t => (
                    Identity: t.CustomerIdentifiers.FirstOrDefault(x => x.IdentityScheme == Identity.Types.IdentitySchemes.Kb),
                    CustomerOnSA: default(_HO.CustomerOnSA),
                    Role: t.RelationshipCustomerProductTypeId,
                    Agent: t.Agent ?? false
                 ))
                .ToList();
        }

        // detail customeru z customerService
        var identifiedCustomers = customerIdentities.Where(t => t.Identity is not null).ToList();
        var customerDetails = new List<_Cust.CustomerDetailResponse>();
        if (identifiedCustomers.Any())
        {
            customerDetails = ServiceCallResult.ResolveAndThrowIfError<_Cust.CustomerListResponse>(
                await _customerService.GetCustomerList(identifiedCustomers.Select(t => t.Identity!), cancellationToken)
            ).Customers.ToList();
        }

        return customerIdentities.Select(t =>
        {
            var customer = t.Identity is null ? new _Cust.CustomerDetailResponse
            {
                NaturalPerson = new _Cust.NaturalPerson
                {
                    FirstName = t.CustomerOnSA!.FirstNameNaturalPerson,
                    LastName = t.CustomerOnSA.Name,
                    DateOfBirth = t.CustomerOnSA.DateOfBirthNaturalPerson
                }
            } : customerDetails.First(x => x.Identity.IdentityId == t.Identity.IdentityId && x.Identity.IdentityScheme == t.Identity.IdentityScheme);
            var permanentAddress = customer.Addresses?.FirstOrDefault(x => x.AddressTypeId == (int)CIS.Foms.Enums.AddressTypes.Permanent);
            var mailingAddress = customer.Addresses?.FirstOrDefault(x => x.AddressTypeId == (int)CIS.Foms.Enums.AddressTypes.Mailing);
            var country = countries.FirstOrDefault(x => x.Id == customer.NaturalPerson.CitizenshipCountriesId.FirstOrDefault());

            return new GetCustomersResponseCustomer
            {
                Agent = t.Agent,
                Email = customer.Contacts?.FirstOrDefault(x => x.ContactTypeId == (int)CIS.Foms.Enums.ContactTypes.Email)?.Value,
                Mobile = customer.Contacts?.FirstOrDefault(x => x.ContactTypeId == (int)CIS.Foms.Enums.ContactTypes.Mobil)?.Value,
                KBID = customer.Identity?.IdentityId.ToString(System.Globalization.CultureInfo.InvariantCulture),
                RoleName = ((CIS.Foms.Enums.CustomerRoles)t.Role).GetAttribute<DisplayAttribute>()!.Name,
                DateOfBirth = customer.NaturalPerson?.DateOfBirth,
                LastName = customer.NaturalPerson?.LastName,
                FirstName = customer.NaturalPerson?.FirstName,
                PermanentAddress = permanentAddress,
                ContactAddress = mailingAddress,
                CitizenshipCountry = country is null ? null : new()
                {
                    Id = country.Id,
                    Name = country?.Name
                }
            };
        }).ToList();
    }

    private static int[] _allowedCustomerRoles = new[] { 1, 2 };
    private static List<int>? _allowedSalesArrangementTypes;

    private readonly DomainServices.ProductService.Clients.IProductServiceClient _productService;
    private readonly DomainServices.CustomerService.Clients.ICustomerServiceClient _customerService;
    private readonly DomainServices.CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient _customerOnSAService;
    private readonly DomainServices.CaseService.Clients.ICaseServiceClient _caseService;

    public GetCustomersHandler(
        DomainServices.ProductService.Clients.IProductServiceClient productService,
        DomainServices.CustomerService.Clients.ICustomerServiceClient customerService,
        DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient customerOnSAService,
        DomainServices.CodebookService.Clients.ICodebookServiceClients codebookService,
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
