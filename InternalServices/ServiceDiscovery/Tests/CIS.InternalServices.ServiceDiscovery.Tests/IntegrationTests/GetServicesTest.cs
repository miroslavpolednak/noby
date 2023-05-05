using CIS.InternalServices.ServiceDiscovery.Tests.IntegrationTests.Helpers;
using CIS.Testing;
using Grpc.Core;

namespace CIS.InternalServices.ServiceDiscovery.Tests.IntegrationTests;

public sealed class GetServicesTest
    : IntegrationTestBase
{
    [Fact]
    public async Task GetServices_ShouldReturnAllServices()
    {
        this.SeedDatabaseWithServices();

        var client = CreateGrpcClient();
        var result = await client.GetServicesAsync(new Contracts.GetServicesRequest
        {
            Environment = Constants.ServicesEnvironmentName1,
            ServiceType = Contracts.ServiceTypes.Unknown
        }, default);

        result.Services.Should().NotBeEmpty().And.HaveCount(3);
        result.Services.Should().Contain(new Contracts.DiscoverableService
        {
            ServiceName = "DS:Service1",
            ServiceType = Contracts.ServiceTypes.Grpc,
            ServiceUrl = "http://0.0.0.0:1"
        });
        result.Services.Should().ContainSingle(t => t.ServiceName == "DS:Service1" && t.ServiceType == Contracts.ServiceTypes.Grpc);
        result.Services.Should().ContainSingle(t => t.ServiceType == Contracts.ServiceTypes.Proprietary);
    }

    [Fact]
    public async Task GetServices_ShouldReturnGrpcServices()
    {
        this.SeedDatabaseWithServices();

        var client = CreateGrpcClient();
        var result = await client.GetServicesAsync(new Contracts.GetServicesRequest
        {
            Environment = Constants.ServicesEnvironmentName1,
            ServiceType = Contracts.ServiceTypes.Grpc
        }, default);

        result.Services.Should().NotBeEmpty().And.HaveCount(2);
    }

    [Fact]
    public async Task GetServices_UnknownEnvironment_ShouldThrowNotFound()
    {
        this.SeedDatabaseWithServices();

        var client = CreateGrpcClient();
        Func<Task> act = async () => await client.GetServicesAsync(new Contracts.GetServicesRequest
        {
            Environment = "unkown",
            ServiceType = Contracts.ServiceTypes.Grpc
        }, default);

        await act.Should().ThrowAsync<RpcException>().Where(t => t.StatusCode == StatusCode.NotFound);
    }

    public GetServicesTest(WebApplicationFactoryFixture<Program> fixture)
        : base(fixture)
    {
    }
}
