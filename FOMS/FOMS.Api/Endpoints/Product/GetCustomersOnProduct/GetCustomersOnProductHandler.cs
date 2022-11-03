using _Cust = DomainServices.CustomerService.Contracts;
using _Pr = DomainServices.ProductService.Contracts;

namespace FOMS.Api.Endpoints.Product.GetCustomersOnProduct;

internal sealed class GetCustomersOnProductHandler
    : IRequestHandler<GetCustomersOnProductRequest, List<GetCustomersOnProductCustomer>>
{
    public async Task<List<GetCustomersOnProductCustomer>> Handle(GetCustomersOnProductRequest request, CancellationToken cancellationToken)
    {
        // dostat seznam klientu z konsDb
        var customers = ServiceCallResult.ResolveAndThrowIfError<_Pr.GetCustomersOnProductResponse>(await _productService.GetCustomersOnProduct(request.CaseId, cancellationToken));

        // detail customeru z customerService
        var identifiedCustomers = customers.Customers.Where(t => t.CustomerIdentifiers is not null && t.CustomerIdentifiers.Any(x => x.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb)).ToList();
        var customerDetails = new List<_Cust.CustomerDetailResponse>();
        if (identifiedCustomers.Any())
        {
            customerDetails = ServiceCallResult.ResolveAndThrowIfError<_Cust.CustomerListResponse>(
                await _customerService.GetCustomerList(identifiedCustomers.Select(t => t.CustomerIdentifiers.First(x => x.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb)), cancellationToken)
            ).Customers.ToList();
        }

        return customers.Customers.Select(t =>
        {
            var c = customerDetails.FirstOrDefault(x => x.Identity.IdentityId == t.CustomerIdentifiers.FirstOrDefault(x2 => x2.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb)?.IdentityId);

            return new GetCustomersOnProductCustomer
            {
                Identities = t.CustomerIdentifiers.Select(x => (CIS.Foms.Types.CustomerIdentity)x!).ToList(),
                FirstName = c?.NaturalPerson?.FirstName,
                LastName = c?.NaturalPerson?.LastName,
                DateOfBirth = c?.NaturalPerson?.DateOfBirth,
                IdentificationDocument = c?.IdentificationDocument is null ? null : new()
                {
                    IdentificationDocumentTypeId = c.IdentificationDocument.IdentificationDocumentTypeId,
                    Number = c.IdentificationDocument.Number
                }
            };
        })
            .ToList();
    }

    private readonly DomainServices.ProductService.Clients.IProductServiceClient _productService;
    private readonly DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction _customerService;

    public GetCustomersOnProductHandler(
        DomainServices.ProductService.Clients.IProductServiceClient productService, 
        DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction customerService)
    {
        _productService = productService;
        _customerService = customerService;
    }
}
