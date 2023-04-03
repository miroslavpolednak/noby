using NSubstitute;
using static DomainServices.DocumentArchiveService.Contracts.v1.DocumentArchiveService;

namespace DomainServices.DocumentArchiveService.Tests.IntegrationTests.Helpers;
public class IntegrationTestBase : IClassFixture<GrpcTestFixture<Program>>
{

    public IntegrationTestBase(GrpcTestFixture<Program> fixture)
    {
        Fixture = fixture;
    }

    public GrpcTestFixture<Program> Fixture { get; }

    protected DocumentArchiveServiceClient CreateClient()
    {
        return new DocumentArchiveServiceClient(Fixture.GrpcChannel);
    }
}
