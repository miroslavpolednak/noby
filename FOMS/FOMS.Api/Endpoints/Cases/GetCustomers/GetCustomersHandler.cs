using _Case = DomainServices.CaseService.Contracts;
using _SA = DomainServices.SalesArrangementService.Contracts;
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

        List<(Identity Identity, int Role)> customerIdentities;

        if (caseInstance.State == 1)
        {
            // get allowed SA types
            if (_allowedSalesArrangementTypes is null)
                _allowedSalesArrangementTypes = (await _codebookService.SalesArrangementTypes(cancellationToken))
                    .Where(t => t.ProductTypeId.GetValueOrDefault() > 0).Select(t => t.Id)
                    .ToList();

            // get salesArrangementId
            var saInstances = ServiceCallResult.ResolveAndThrowIfError<_SA.GetSalesArrangementListResponse>(await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellationToken: cancellationToken));
            var saId = saInstances.SalesArrangements.First(t => _allowedSalesArrangementTypes.Contains(t.SalesArrangementTypeId)).SalesArrangementId;

            // get filtered customers
            var customers = ServiceCallResult.ResolveAndThrowIfError<List<_SA.CustomerOnSA>>(await _customerOnSAService.GetCustomerList(saId, cancellationToken))
                .Where(t => _allowedCustomerRoles.Contains(t.CustomerRoleId) && t.CustomerIdentifiers is not null && t.CustomerIdentifiers.Any(x => x.IdentityScheme == Identity.Types.IdentitySchemes.Kb));

            customerIdentities = customers.Select(t => (t.CustomerIdentifiers.First(x => x.IdentityScheme == Identity.Types.IdentitySchemes.Kb), t.CustomerRoleId)).ToList();
        }
        else
        {
            var customers = ServiceCallResult.ResolveAndThrowIfError<DomainServices.ProductService.Contracts.GetCustomersOnProductResponse>(await _productService.GetCustomersOnProduct(request.CaseId, cancellationToken));
            customerIdentities = customers.Customers
                .Select(t => (Identity: t.CustomerIdentifiers.First(x => x.IdentityScheme == Identity.Types.IdentitySchemes.Kb), Role: t.RelationshipCustomerProductTypeId))
                .ToList();
        }

        // detail customeru z customerService
        var customerDetails = ServiceCallResult.ResolveAndThrowIfError<_Cust.CustomerListResponse>(await _customerService.GetCustomerList(customerIdentities.Select(t => t.Identity), cancellationToken));
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
    private readonly DomainServices.SalesArrangementService.Abstraction.ICustomerOnSAServiceAbstraction _customerOnSAService;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;

    public GetCustomersHandler(
        DomainServices.ProductService.Abstraction.IProductServiceAbstraction productService,
        DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction customerService,
        DomainServices.SalesArrangementService.Abstraction.ICustomerOnSAServiceAbstraction customerOnSAService,
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
