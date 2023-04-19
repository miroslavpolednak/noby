using CIS.Infrastructure.gRPC.CisTypes;
using CIS.Testing;
using DomainServices.DocumentArchiveService.Tests.IntegrationTests.Helpers;


namespace DomainServices.DocumentArchiveService.Tests.IntegrationTests;
public class GetDocumentTests : IntegrationTestBase
{
    public GetDocumentTests(WebApplicationFactoryFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task GetDocumentTests_FromTcp_WithContent_ShouldReturnTcpMockData()
    {
        var client = CreateGrpcClient();

        var response = await client.GetDocumentAsync(new() { DocumentId = "NotSdfPrefixMockedId", WithContent = true }, default);

        response.Should().NotBeNull();
        response.Content.Should().NotBeNull();
        response.Content.BinaryData.ToBase64().Should().Be("VGhpcyBpcyBhIHRlc3Q=");

        response.Metadata.CaseId.Should().Be(1);
        response.Metadata.DocumentId.Should().Be("TestID");
        response.Metadata.EaCodeMainId.Should().Be(3);
        response.Metadata.Filename.Should().Be("test.txt");
        response.Metadata.Description.Should().Be("test.txt");
        response.Metadata.OrderId.Should().Be(1);
        response.Metadata.CreatedOn.Should().Be(new GrpcDate(new DateTime(2000, 1, 1)));
        response.Metadata.AuthorUserLogin.Should().Be("TestLogin");
        response.Metadata.Priority.Should().Be("1");
        response.Metadata.Status.Should().Be("Z");
        response.Metadata.FolderDocument.Should().Be("S");
        response.Metadata.FolderDocumentId.Should().Be("P");
        response.Metadata.DocumentDirection.Should().Be("E");
        response.Metadata.FormId.Should().Be("FormTest1");
        response.Metadata.ContractNumber.Should().Be("132");
        response.Metadata.PledgeAgreementNumber.Should().Be("11111");
        response.Metadata.MinorCodes.First().Should().Be(1);
        response.Content.MineType.Should().Be("text/plain");
        response.Metadata.Completeness.Should().Be(1);
    }

    [Fact]
    public async Task GetDocumentTests_FromSdf_WithoutData_ShouldReturnExternalServiceMockData()
    {
        var client = CreateGrpcClient();

        var response = await client.GetDocumentAsync(new() { DocumentId = "KBHMockedId", WithContent = false }, default);

        response.Should().NotBeNull();
        response.Content.BinaryData.Should().BeEmpty();
        response.Metadata.Should().NotBeNull();
        response.Metadata.CaseId.Should().Be(132456L);
        response.Metadata.DocumentId.Should().Be("TestDocId");
        response.Metadata.EaCodeMainId.Should().Be(603225);
        response.Metadata.Filename.Should().Be("TestFilename.txt");
        response.Metadata.Description.Should().Be("TestDescription");
        response.Metadata.OrderId.Should().Be(11);
        response.Metadata.CreatedOn.Should().Be(new GrpcDate(new DateTime(2000, 1, 1)));
        response.Metadata.AuthorUserLogin.Should().Be("TestAuthor");
        response.Metadata.Priority.Should().Be("TestPriority");
        response.Metadata.Status.Should().Be("TestStatus");
        response.Metadata.FolderDocument.Should().Be("TestFolderDocument");
        response.Metadata.FolderDocumentId.Should().Be("TestFolderDocumentId");
        response.Metadata.DocumentDirection.Should().Be("TestDocumentDirection");
        response.Metadata.SourceSystem.Should().Be("TestSourceSystem");
        response.Metadata.FormId.Should().Be("TestFormId");
        response.Metadata.ContractNumber.Should().Be("TestContractNumber");
        response.Metadata.PledgeAgreementNumber.Should().Be("TestPledgeAgreementNumber");
    }
}
