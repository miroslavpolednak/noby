using CIS.Testing;
using static CIS.InternalServices.ServiceDiscovery.Contracts.v1.DiscoveryService;

namespace CIS.InternalServices.ServiceDiscovery.Tests.IntegrationTests.Helpers;

public abstract class IntegrationTestBase : IClassFixture<WebApplicationFactoryFixture<Program>>
{
    public IntegrationTestBase(WebApplicationFactoryFixture<Program> fixture)
    {
        Fixture = fixture;

        ConfigureWebHost();

        CreateGlobalMocks();
    }

    public WebApplicationFactoryFixture<Program> Fixture { get; }

    protected DiscoveryServiceClient CreateGrpcClient()
    {
        return Fixture.CreateGrpcClient<DiscoveryServiceClient>();
    }

    private void ConfigureWebHost()
    {
        Fixture
           .ConfigureCisTestOptions(options =>
           {
               options.Header = new() { { "test", "Test" } }; // default is null
           })
           .ConfigureServices(services =>
           {
               // Example of manual register of db context with inmemory database
               //var dbName = Guid.NewGuid().ToString();// unique db name for every test class
               //services.RemoveAll<DbContextOptions<DocumentArchiveDbContext>>()
               //      .AddDbContext<DocumentArchiveDbContext>(options =>
               //      {
               //          options.UseInMemoryDatabase(dbName);
               //          options.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
               //      });
           });
    }

    private void CreateGlobalMocks()
    {
       // Some global mocks
    }
}
