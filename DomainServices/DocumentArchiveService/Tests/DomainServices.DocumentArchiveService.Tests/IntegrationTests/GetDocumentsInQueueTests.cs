using DomainServices.DocumentArchiveService.Api.Database;
using DomainServices.DocumentArchiveService.Tests.IntegrationTests.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.DocumentArchiveService.Tests.IntegrationTests;
public class GetDocumentsInQueueTests : IntegrationTestBase
{
    public GetDocumentsInQueueTests(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task GetDocumentsInQueueTest_ShouldReturnDocuments()
    {
        // Prepare test data
        var docId = "KBHXXD00000000000000000000001";
        using var scope = Fixture.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DocumentArchiveDbContext>();
        db.DocumentInterface.Add(CreateDocumentInterfaceEntity(documentId: docId));
        db.SaveChanges();

        var client = CreateGrpcClient();

        var request = new Contracts.GetDocumentsInQueueRequest();
        request.EArchivIds.Add(docId);

        var response = await client.GetDocumentsInQueueAsync(request, default);
        request.Should().NotBeNull();
        response.QueuedDocuments.Should().HaveCount(1);
        response.QueuedDocuments.First().EArchivId.Should().Be(docId);
    }

}
