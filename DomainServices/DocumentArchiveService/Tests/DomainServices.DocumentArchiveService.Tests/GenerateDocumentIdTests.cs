using DomainServices.DocumentArchiveService.Tests.IntegrationTests.Helpers;
using FluentAssertions;
using NSubstitute;

namespace DomainServices.DocumentArchiveService.Tests;
public class GenerateDocumentIdTests : IntegrationTestBase
{
    public GenerateDocumentIdTests(GrpcTestFixture<Program> fixture)
       : base(fixture)
    {
        // Mock external dependencies
        Fixture.DocumentSequenceRepository.GetNextDocumentSeqValue(Arg.Any<CancellationToken>()).Returns(1);
    }

    [Fact]
    public async Task CreateDocumentId_Should_Success()
    {
        var client = CreateClient();

        var response = await client.GenerateDocumentIdAsync(new(), default);

        response.DocumentId.Should().NotBeEmpty();
        response.DocumentId.Should().Contain("1");
    }
}
