using _Cust = DomainServices.CustomerService.Contracts;

namespace NOBY.Api.Endpoints.Product.GetCustomersOnProduct;

internal sealed class GetCustomersOnProductHandler(
    DomainServices.ProductService.Clients.IProductServiceClient _productService,
    DomainServices.CustomerService.Clients.v1.ICustomerServiceClient _customerService)
        : IRequestHandler<GetCustomersOnProductRequest, List<ProductGetCustomersOnProductItem>>
{
    public async Task<List<ProductGetCustomersOnProductItem>> Handle(GetCustomersOnProductRequest request, CancellationToken cancellationToken)
    {
        // dostat seznam klientu z konsDb
        var customers = (await _productService.GetCustomersOnProduct(request.CaseId, cancellationToken))
            .Customers
            .Where(t => t.RelationshipCustomerProductTypeId is 1 or 2)
            .ToList();

        // detail customeru z customerService
        var identifiedCustomers = customers
            .Where(t => t.CustomerIdentifiers is not null && t.CustomerIdentifiers.HasKbIdentity())
            .ToList();

        var customerDetails = new List<_Cust.Customer>();
        if (identifiedCustomers.Count != 0)
        {
            customerDetails = (await _customerService.GetCustomerList(identifiedCustomers.Select(t => t.CustomerIdentifiers.GetKbIdentity()), cancellationToken)).Customers.ToList();
        }

        return customers.Select(t =>
        {
            var c = customerDetails.FirstOrDefault(x => x.Identities.GetKbIdentity().IdentityId == t.CustomerIdentifiers.GetKbIdentityOrDefault()?.IdentityId);

            return new ProductGetCustomersOnProductItem
            {
                Identities = t.CustomerIdentifiers.Select(x => (SharedTypesCustomerIdentity)x!).ToList(),
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
}
