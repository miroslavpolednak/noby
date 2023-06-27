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
    public async Task GetDocumentsInQueueTest_WithoutDocumentData_ShouldReturnDocuments()
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
        response.Should().NotBeNull();
        response.QueuedDocuments.Should().HaveCount(1);
        response.QueuedDocuments.First().EArchivId.Should().Be(docId);
        response.QueuedDocuments.First().Description.Should().Be(docEntity.Description);
        response.QueuedDocuments.First().CreatedOn.Year.Should().Be(docEntity.CreatedOn.Year);
        response.QueuedDocuments.First().CreatedOn.Month.Should().Be(docEntity.CreatedOn.Month);
        response.QueuedDocuments.First().CreatedOn.Day.Should().Be(docEntity.CreatedOn.Day);
        response.QueuedDocuments.First().DocumentData.Length.Should().Be(0);
    }

    [Fact]
    public async Task GetDocumentsInQueue_WithBinaryData_ShouldReturnBinaryData()
    {
        // Prepare test data
        var docId = "KBHXXD00000000000000000000002";
        using var scope = Fixture.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DocumentArchiveDbContext>();

        var docEntity = CreateDocumentInterfaceEntity(documentId: docId);
        db.DocumentInterface.Add(docEntity);
        db.SaveChanges();

        var client = CreateGrpcClient();

        var request = new Contracts.GetDocumentsInQueueRequest();
        request.EArchivIds.Add(docId);
        request.WithContent = true;

        var response = await client.GetDocumentsInQueueAsync(request, default);
        response.Should().NotBeNull();
        response.QueuedDocuments.First().DocumentData.Length.Should().BeGreaterThan(1);
    }
}
