using _Case = DomainServices.CaseService.Contracts;
using _SA = DomainServices.SalesArrangementService.Contracts;
using _HO = DomainServices.HouseholdService.Contracts;
using _Cust = DomainServices.CustomerService.Contracts;
using CIS.Infrastructure.gRPC.CisTypes;

namespace FOMS.Api.Endpoints.Cases.GetCustomers;

internal class GetCustomersHandler
    : IRequestHandler<GetCustomersRequest, List<GetCustomersResponseCustomer>>
{
    public async Task<List<GetCustomersResponseCustomer>> Handle(GetCustomersRequest request, CancellationToken cancellationToken)
    {
        // data o CASE-u
        var caseInstance = ServiceCallResult.ResolveAndThrowIfError<_Case.Case>(await _caseService.GetCaseDetail(request.CaseId, cancellationToken));

        List<(Identity Identity, int Role, bool Agent)> customerIdentities;

        if (caseInstance.State == 1)
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
            var customers = ServiceCallResult.ResolveAndThrowIfError<List<_HO.CustomerOnSA>>(
                await _customerOnSAService.GetCustomerList(saId, cancellationToken)
            );

            // vybrat a transformovat jen vlastnik, spoludluznik
            customerIdentities = customers
                .Where(t => _allowedCustomerRoles.Contains(t.CustomerRoleId) && t.CustomerIdentifiers is not null && t.CustomerIdentifiers.Any(x => x.IdentityScheme == Identity.Types.IdentitySchemes.Kb))
                .Select(t => (
                    t.CustomerIdentifiers.First(x => x.IdentityScheme == Identity.Types.IdentitySchemes.Kb), 
                    t.CustomerRoleId,
                    saDetail.Mortgage.Agent.GetValueOrDefault() == t.CustomerOnSAId
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
                    Identity: t.CustomerIdentifiers.First(x => x.IdentityScheme == Identity.Types.IdentitySchemes.Kb), 
                    Role: t.RelationshipCustomerProductTypeId,
                    Agent: t.Agent ?? false
                 ))
                .ToList();
        }

        // detail customeru z customerService
        var customerDetails = ServiceCallResult.ResolveAndThrowIfError<_Cust.CustomerListResponse>(
            await _customerService.GetCustomerList(customerIdentities.Select(t => t.Identity), cancellationToken)
        );
        // seznam zemi
        var countries = (await _codebookService.Countries(cancellationToken));

        return customerDetails.Customers.Select(t => t.ToApiResponse(customerIdentities, countries)).ToList();
    }

    private static int[] _allowedCustomerRoles = new[] { 1, 2 };
    private static List<int>? _allowedSalesArrangementTypes;

    private readonly DomainServices.ProductService.Abstraction.IProductServiceAbstraction _productService;
    private readonly DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction _customerService;
    private readonly DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient _customerOnSAService;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;

    public GetCustomersHandler(
        DomainServices.ProductService.Abstraction.IProductServiceAbstraction productService,
        DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction customerService,
        DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient customerOnSAService,
        DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService, 
        DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _productService = productService;
        _customerService = customerService;
        _customerOnSAService = customerOnSAService;
        _codebookService = codebookService;
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
    }
}
