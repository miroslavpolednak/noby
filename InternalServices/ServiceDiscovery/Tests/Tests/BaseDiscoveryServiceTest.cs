using ProtoBuf.Grpc;
using CIS.Testing;
using Grpc.Core;
using CIS.Core.Types;

namespace CIS.InternalServices.ServiceDiscovery.Tests;

public abstract class BaseDiscoveryServiceTest
{
    [Fact]
    public async Task GetServices_ShouldReturnAll()
    {
        // arrange
        var request = new Contracts.GetServicesRequest() { Environment = Constants.Environment };

        // act
        var result = await _svc.GetServicesAsync(request);

        // assert
        Assert.True(result.Services.Count() > 0);
    }

    [Fact]
    public async Task GetAllServices_InvalidEnvironment_ShouldThrowException()
    {
        // arrange
        var request = new Contracts.GetServicesRequest() { Environment = "invalidenv" };

        // assert, act
        var exception = await Assert.ThrowsAsync<RpcException>(async () => await _svc.GetServicesAsync(request));
        Assert.True(exception.StatusCode == StatusCode.NotFound);
        Assert.Equal(102, exception?.Trailers.GetInt32(Core.Exceptions.ExceptionHandlingConstants.GrpcTrailerCisCodeKey).GetValueOrDefault());
    }

    [Fact]
    public async Task GetService_ShouldReturn()
    {
        // arrange
        string name = new ServiceName(ServiceName.WellKnownServices.Redis);
        var request = new Contracts.GetServiceRequest() { Environment = Constants.Environment, ServiceType = Contracts.ServiceTypes.Grpc, ServiceName = name };

        // act
        var result = await _svc.GetServiceAsync(request);

        // assert
        Assert.Equal(name, result.Service.ServiceName);
    }

    [Fact]
    public async Task GetService_InvalidServiceName_ShouldThrowExceptionNotFound()
    {
        // arrange
        var request = new Contracts.GetServiceRequest() { Environment = Constants.Environment, ServiceType = Contracts.ServiceTypes.Grpc, ServiceName = "invalid:name" };

        // assert, act
        var exception = await Assert.ThrowsAsync<RpcException>(async () => await _svc.GetServiceAsync(request));
        Assert.True(exception.StatusCode == StatusCode.NotFound);
        Assert.Equal(103, exception?.Trailers.GetInt32(Core.Exceptions.ExceptionHandlingConstants.GrpcTrailerCisCodeKey).GetValueOrDefault());
    }

    [Fact]
    public async Task GetService_InvalidServiceName_ShouldThrowExceptionInvalidArgument()
    {
        // arrange
        var request = new Contracts.GetServiceRequest() { Environment = Constants.Environment, ServiceName = "invalid_name" };

        // assert, act
        var exception = await Assert.ThrowsAsync<RpcException>(async () => await _svc.GetServiceAsync(request));
        Assert.True(exception.StatusCode == StatusCode.Internal);
    }

    protected readonly Contracts.v1.DiscoveryService.DiscoveryServiceClient _svc;
    protected readonly TestFixture<Program> _testFixture;
    
    public BaseDiscoveryServiceTest(TestFixture<Program> testFixture)
    {
        _testFixture = testFixture
            .Recreate()
            .Init(this)
            .ConfigureTestDatabase(options =>
            {
                options.SeedPaths = "~/DiscoveryServiceDatabaseSeed.sql";
            });

        _svc = new Contracts.v1.DiscoveryService.DiscoveryServiceClient(_testFixture.GetGrpcChannel());
    }
}
