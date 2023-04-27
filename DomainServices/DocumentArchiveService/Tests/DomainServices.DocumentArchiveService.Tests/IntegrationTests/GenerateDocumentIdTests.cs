using CIS.Testing;
using DomainServices.DocumentArchiveService.Tests.IntegrationTests.Helpers;
using NSubstitute;

namespace DomainServices.DocumentArchiveService.Tests.IntegrationTests;
public class GenerateDocumentIdTests : IntegrationTestBase
{
    public GenerateDocumentIdTests(WebApplicationFactoryFixture<Program> fixture)
       : base(fixture)
    {
        // Mock external dependencies
        DocumentSequenceRepository.GetNextDocumentSeqValue(Arg.Any<CancellationToken>()).Returns(1);
    }

    [Fact]
    public async Task CreateDocumentId_Should_Success()
    {
        var client = CreateGrpcClient();

        var response = await client.GenerateDocumentIdAsync(new(), default);

        response.DocumentId.Should().NotBeEmpty();
        response.DocumentId.Should().Contain("1");
        response.DocumentId.Should().Contain("KBH");
        response.DocumentId.Should().Contain("XX");
        response.DocumentId.Should().Contain("T");
        response.DocumentId.Should().HaveLength(29);
    }
}
