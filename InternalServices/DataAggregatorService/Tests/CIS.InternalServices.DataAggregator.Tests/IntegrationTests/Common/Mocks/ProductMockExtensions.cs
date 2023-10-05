using SharedTypes.GrpcTypes;
using CIS.Testing.Common;
using DomainServices.ProductService.Clients;
using DomainServices.ProductService.Contracts;

namespace CIS.InternalServices.DataAggregator.Tests.IntegrationTests.Common.Mocks;

public static class ProductMockExtensions
{
    public static void MockGetCustomersOnProduct(this IProductServiceClient productService, Identity applicant)
    {
        var fixture = FixtureFactory.Create();

        var customerOnProduct = fixture.Create<GetCustomersOnProductResponseItem>();
        customerOnProduct.CustomerIdentifiers.Add(applicant);

        productService.GetCustomersOnProduct(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns(new GetCustomersOnProductResponse { Customers = { customerOnProduct } });
    }
}