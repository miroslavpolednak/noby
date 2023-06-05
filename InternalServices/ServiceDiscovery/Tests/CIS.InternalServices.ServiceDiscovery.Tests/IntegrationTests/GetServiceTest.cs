using CIS.InternalServices.ServiceDiscovery.Tests.IntegrationTests.Helpers;
using CIS.Testing;
using Grpc.Core;

namespace CIS.InternalServices.ServiceDiscovery.Tests.IntegrationTests;

public class GetServiceTest
    : IntegrationTestBase
{
    [Fact]
    public async Task GetService_ShouldReturnService()
    {
        this.SeedDatabaseWithServices();

        var client = CreateGrpcClient();
        var result = await client.GetServiceAsync(new Contracts.GetServiceRequest
        {
            ServiceName = "DS:Service1",
            Environment = Constants.ServicesEnvironmentName1,
            ServiceType = Contracts.ServiceTypes.Grpc
        }, default);

        result.EnvironmentName.Should().Be(Constants.ServicesEnvironmentName1);
        result.Service.Should().Be(new Contracts.DiscoverableService
        {
            ServiceName = "DS:Service1",
            ServiceType = Contracts.ServiceTypes.Grpc,
            ServiceUrl = "http://0.0.0.0:1"
        });
    }

    [Fact]
    public async Task GetService_UnknownEnvironment_ShouldThrowNotFound()
    {
        this.SeedDatabaseWithServices();

        var client = CreateGrpcClient();
        Func<Task> act = async () => await client.GetServiceAsync(new Contracts.GetServiceRequest
        {
            ServiceName = "DS:Service1",
            Environment = "unkown",
            ServiceType = Contracts.ServiceTypes.Grpc
        }, default);

        await act.Should().ThrowAsync<RpcException>().Where(t => t.StatusCode == StatusCode.NotFound);
    }

    [Fact]
    public async Task GetService_UnknownService_ShouldThrowNotFound()
    {
        this.SeedDatabaseWithServices();

        var client = CreateGrpcClient();
        Func<Task> act = async () => await client.GetServiceAsync(new Contracts.GetServiceRequest
        {
            ServiceName = "DS:Service_unknown",
            Environment = Constants.ServicesEnvironmentName1,
            ServiceType = Contracts.ServiceTypes.Grpc
        }, default);

        await act.Should().ThrowAsync<RpcException>().Where(t => t.StatusCode == StatusCode.NotFound);
    }

    [Fact]
    public async Task GetService_ServiceTypeNotFound_ShouldThrowNotFound()
    {
        this.SeedDatabaseWithServices();

        var client = CreateGrpcClient();
        Func<Task> act = async () => await client.GetServiceAsync(new Contracts.GetServiceRequest
        {
            ServiceName = "DS:Service1",
            Environment = Constants.ServicesEnvironmentName1,
            ServiceType = Contracts.ServiceTypes.Rest
        }, default);

        await act.Should().ThrowAsync<RpcException>().Where(t => t.StatusCode == StatusCode.NotFound);
    }

    public GetServiceTest(WebApplicationFactoryFixture<Program> fixture)
        : base(fixture)
    {
    }
}
