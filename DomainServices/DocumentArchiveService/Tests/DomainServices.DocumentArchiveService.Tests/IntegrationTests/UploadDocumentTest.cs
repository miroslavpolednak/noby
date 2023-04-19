using AutoFixture.Xunit2;
using CIS.Testing;
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
        entity!.CaseId.Should().Be(request.Metadata.CaseId);
        entity.DocumentId.Should().Be(request.Metadata.DocumentId);
        entity.EaCodeMainId.Should().Be(request.Metadata.EaCodeMainId);
        entity.FileName.Should().Be(request.Metadata.Filename);
        entity.AuthorUserLogin.Should().Be(request.Metadata.AuthorUserLogin);
        entity.CreatedOn.Should().Be(request.Metadata.CreatedOn);
        entity!.Description.Should().Be(request.Metadata.Description);
        entity.FormId.Should().Be(request.Metadata.FormId);
        entity.ContractNumber.Should().Be(request.Metadata.ContractNumber);
        entity.DocumentDirection.Should().Be(request.Metadata.DocumentDirection);
        entity.FolderDocument.Should().Be(request.Metadata.FolderDocument);
    }
}
