using CIS.Infrastructure.gRPC;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using static DomainServices.DocumentArchiveService.Contracts.v1.DocumentArchiveService;

namespace DomainServices.DocumentArchiveService.Tests.IntegrationTests.Helpers;

public abstract class IntegrationTestBase : IClassFixture<GrpcTestFixture<Program>>
{
    public IntegrationTestBase(GrpcTestFixture<Program> fixture)
    {
        Fixture = fixture;
    }

    public GrpcTestFixture<Program> Fixture { get; }

    protected DocumentArchiveServiceClient CreateClient()
    {
        var channel = Fixture.GrpcChannel;
        // Example of how to set interceptor, if you don't want use interceptor, just pass channel to client instance instead of invoker
        // If you register this GenericClientExceptionInterceptor you will get our CisException instead of default RpcException
        var exceptionInterceptor = Fixture.Services.GetRequiredService<GenericClientExceptionInterceptor>();
        var invoker = channel.Intercept(exceptionInterceptor);

        return new DocumentArchiveServiceClient(invoker);
    }
}
