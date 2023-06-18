using CIS.Testing.Database;
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Clients.Services;
using DomainServices.CustomerService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.OfferService.Clients;
using DomainServices.ProductService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.UserService.Clients.Services;
using DomainServices.UserService.Clients;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using static CIS.InternalServices.DataAggregatorService.Contracts.V1.DataAggregatorService;

namespace CIS.InternalServices.DataAggregator.Tests.IntegrationTests.Common;

public class IntegrationTestBase
{
    protected ISalesArrangementServiceClient SalesArrangementServiceClient { get; } = Substitute.For<ISalesArrangementServiceClient>();
    protected ICaseServiceClient CaseServiceClient { get; } = Substitute.For<ICaseServiceClient>();
    protected IOfferServiceClient OfferServiceClient { get; } = Substitute.For<IOfferServiceClient>();
    protected ICustomerServiceClient CustomerServiceClient { get; } = Substitute.For<ICustomerServiceClient>();
    protected IProductServiceClient ProductServiceClient { get; } = Substitute.For<IProductServiceClient>();
    protected IHouseholdServiceClient HouseholdServiceClient { get; } = Substitute.For<IHouseholdServiceClient>();
    protected ICustomerOnSAServiceClient CustomerOnSAServiceClient { get; } = Substitute.For<ICustomerOnSAServiceClient>();
    protected IDocumentOnSAServiceClient DocumentOnSAServiceClient { get; } = Substitute.For<IDocumentOnSAServiceClient>();

    public IntegrationTestBase()
    {
        Fixture = new WebApplicationFactoryFixture<Program>();

        ConfigureWebHost();
    }

    public WebApplicationFactoryFixture<Program> Fixture { get; }

    protected DataAggregatorServiceClient CreateGrpcClient() => Fixture.CreateGrpcClient<DataAggregatorServiceClient>(true);

    private void ConfigureWebHost()
    {
        Fixture.ConfigureCisTestOptions(opts => opts.DbMockAdapter = new EfInMemoryMockAdapter())
               .ConfigureServices(services =>
               {
                   // This mock is necessary for mock of service discovery
                   services.RemoveAll<IUserServiceClient>().AddSingleton<IUserServiceClient, MockUserService>();

                   services.RemoveAll<ICodebookServiceClient>().AddSingleton<ICodebookServiceClient, CodebookServiceMock>();

                   //Services mocks
                   services.RemoveAll<ISalesArrangementServiceClient>().AddTransient(_ => SalesArrangementServiceClient);
                   services.RemoveAll<ICaseServiceClient>().AddTransient(_ => CaseServiceClient);
                   services.RemoveAll<IOfferServiceClient>().AddTransient(_ => OfferServiceClient);
                   services.RemoveAll<ICustomerServiceClient>().AddTransient(_ => CustomerServiceClient);
                   services.RemoveAll<IProductServiceClient>().AddTransient(_ => ProductServiceClient);
                   services.RemoveAll<IHouseholdServiceClient>().AddTransient(_ => HouseholdServiceClient);
                   services.RemoveAll<ICustomerOnSAServiceClient>().AddTransient(_ => CustomerOnSAServiceClient);
                   services.RemoveAll<IDocumentOnSAServiceClient>().AddTransient(_ => DocumentOnSAServiceClient);
               });
    }
}