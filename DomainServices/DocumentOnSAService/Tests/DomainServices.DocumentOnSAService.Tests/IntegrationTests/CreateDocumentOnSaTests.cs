using CIS.InternalServices.DataAggregatorService.Contracts;
using CIS.Testing;
using DomainServices.DocumentOnSAService.Tests.IntegrationTests.Helpers;
using DomainServices.SalesArrangementService.Contracts;
using FluentAssertions;
using NSubstitute;
using Xunit;
using _dataAgr = CIS.InternalServices.DataAggregatorService.Contracts;

namespace DomainServices.DocumentOnSAService.Tests.IntegrationTests;
public class CreateDocumentOnSaTests : IntegrationTestBase
{
    public CreateDocumentOnSaTests(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task CreateDocumentOnSa_ShouldCreateDocSa_ShouldReturnCorrectData()
    {
        //Mocks
        ArrangementServiceClient.GetSalesArrangement(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new SalesArrangement
        {
            CaseId = 1,
        });

        var resp = new _dataAgr.GetDocumentDataResponse
        {
            DocumentTemplateVariantId = 1,
            DocumentTemplateVersionId = 4,
        };

        DataAggregatorServiceClient.GetDocumentData(Arg.Any<GetDocumentDataRequest>()).ReturnsForAnyArgs(resp);

        var client = CreateGrpcClient();
        var response = await client.CreateDocumentOnSAAsync(new()
        {
            SalesArrangementId = 1,
            DocumentTypeId = 4,
            FormId = "N00000000000101",
            EArchivId = "KBHXXD00000000000000000000351"
        });

        response.Should().NotBeNull();

        //ToDo
    }

    [Fact]
    public async Task CreateDocumentOnSa_PassInvalidParameters_ShouldReturn_ValidationException()
    {
        //ToDo
    }

}
