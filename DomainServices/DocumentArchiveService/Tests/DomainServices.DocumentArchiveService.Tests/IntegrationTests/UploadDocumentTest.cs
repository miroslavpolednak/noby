using DomainServices.DocumentArchiveService.Api.Database;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.DocumentArchiveService.Tests.IntegrationTests.Helpers;
using Google.Protobuf;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.DocumentArchiveService.Tests.IntegrationTests;
public class UploadDocumentTest : IntegrationTestBase
{
    public UploadDocumentTest(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task UploadDocumentTest_ShouldUploadDocument()
    {
        var request = new UploadDocumentRequest
        {
            BinaryData = ByteString.CopyFrom(Convert.FromBase64String("VGhpcyBpcyBhIHRlc3Q=")),
            Metadata = new()
            {
                CaseId = 1,
                DocumentId = "KBHXXD00000000000000000000001",
                EaCodeMainId = 613226,
                Filename = "test.txt",
                AuthorUserLogin = "testUser",
                CreatedOn = new DateTime(2022, 1, 1),
                Description = "Test desc",
                FormId = "N00000000000001",
                ContractNumber = "HF00000000001",
                DocumentDirection = "E",
                FolderDocument = "N"
            }
        };

        var client = CreateGrpcClient();
        var response = await client.UploadDocumentAsync(request);

        // Check data
        var scope = Fixture.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DocumentArchiveDbContext>();

        var entity = db.DocumentInterface.SingleOrDefault(e => e.DocumentId == request.Metadata.DocumentId);
        entity.Should().NotBeNull();
    }
}
