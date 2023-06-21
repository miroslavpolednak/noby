using CIS.Testing;
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

        var docEntity = CreateDocumentInterfaceEntity(documentId: docId);
        db.DocumentInterface.Add(docEntity);
        db.SaveChanges();

        var client = CreateGrpcClient();

        var request = new Contracts.GetDocumentsInQueueRequest();
        request.EArchivIds.Add(docId);

        var response = await client.GetDocumentsInQueueAsync(request, default);
        request.Should().NotBeNull();
        response.QueuedDocuments.Should().HaveCount(1);
        response.QueuedDocuments.First().EArchivId.Should().Be(docId);
        response.QueuedDocuments.First().Description.Should().Be(docEntity.Description);
        response.QueuedDocuments.First().CreatedOn.Year.Should().Be(docEntity.CreatedOn.Year);
        response.QueuedDocuments.First().CreatedOn.Month.Should().Be(docEntity.CreatedOn.Month);
        response.QueuedDocuments.First().CreatedOn.Day.Should().Be(docEntity.CreatedOn.Day);
    }

}
