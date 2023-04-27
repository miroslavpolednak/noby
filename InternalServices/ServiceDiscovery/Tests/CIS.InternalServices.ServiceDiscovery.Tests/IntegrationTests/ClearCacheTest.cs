using CIS.InternalServices.ServiceDiscovery.Tests.IntegrationTests.Helpers;
using CIS.Testing;

namespace CIS.InternalServices.ServiceDiscovery.Tests.IntegrationTests;

public class ClearCacheTest
    : IntegrationTestBase
{
    [Fact]
    public async Task GetService_ShouldReturnService()
    {
        this.SeedDatabaseWithServices();
        string cacheKey = Constants.ServicesEnvironmentName1;

        var client = CreateGrpcClient();
        await client.GetServicesAsync(new Contracts.GetServicesRequest
        {
            Environment = Constants.ServicesEnvironmentName1,
            ServiceType = Contracts.ServiceTypes.Grpc
        }, default);

        Api.Common.ServicesMemoryCache.IsKeyInCache(cacheKey).Should().BeTrue();

        Api.Common.ServicesMemoryCache.Clear();

        Api.Common.ServicesMemoryCache.IsKeyInCache(cacheKey).Should().BeFalse();
    }

    public ClearCacheTest(WebApplicationFactoryFixture<Program> fixture)
        : base(fixture)
    {
    }
}
