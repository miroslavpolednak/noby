using CIS.Core.Exceptions;
using CIS.InternalServices.DataAggregatorService.Contracts;
using CIS.Testing;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Tests.IntegrationTests.Helpers;
using DomainServices.SalesArrangementService.Contracts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;
using _dataAgr = CIS.InternalServices.DataAggregatorService.Contracts;

namespace DomainServices.DocumentOnSAService.Tests.IntegrationTests;

public class CreateDocumentOnSaTests : IntegrationTestBase
{
    public CreateDocumentOnSaTests(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
        //Mocks
        ArrangementServiceClient.GetSalesArrangement(0, Arg.Any<CancellationToken>()).ReturnsForAnyArgs(new SalesArrangement
        {
            CaseId = 1,
            SalesArrangementId = 1,
        });

        var resp = new _dataAgr.GetDocumentDataResponse
        {
            DocumentTemplateVariantId = 1,
            DocumentTemplateVersionId = 4,
        };

        resp.DocumentData.Add(new DocumentFieldData { FieldName = "TestName", Text = "TestText" });

        DataAggregatorServiceClient.GetDocumentData(Arg.Any<GetDocumentDataRequest>()).ReturnsForAnyArgs(resp);
    }

    [Fact]
    public async Task CreateDocumentOnSa_ShouldCreateDocSa_ShouldReturnCorrectData()
    {
        var client = CreateGrpcClient();

        var salesArrangementId = 1;
        var documentTypeId = 4;
        var formId = "N00000000000101";
        var eArchivId = "KBHXXD00000000000000000000351";

        var response = await client.CreateDocumentOnSAAsync(new()
        {
            SalesArrangementId = salesArrangementId,
            DocumentTypeId = documentTypeId,
            FormId = formId,
            EArchivId = eArchivId,
            IsFinal = true,
        });

        response.Should().NotBeNull();
        response.DocumentOnSa.EArchivId.Should().Be(eArchivId);
        response.DocumentOnSa.FormId.Should().Be(formId);
        response.DocumentOnSa.DocumentTypeId.Should().Be(documentTypeId);
        response.DocumentOnSa.SalesArrangementId.Should().Be(salesArrangementId);

        using var scope = Fixture.Services.CreateScope();
        using var dbcontext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();
        var docOnSa = dbcontext.DocumentOnSa.SingleOrDefault(d => d.FormId == formId);
        docOnSa.Should().NotBeNull();
        docOnSa!.IsFinal.Should().BeTrue();
        docOnSa!.Data.Should().NotBeEmpty();
        docOnSa.SalesArrangementId.Should().Be(salesArrangementId);
        docOnSa.EArchivId.Should().Be(eArchivId);
    }

    [Fact]
    public async Task CreateDocumentOnSa_PassInvalidParameters_ShouldReturn_ValidationException()
    {
        var client = CreateGrpcClient();

        Func<Task> act = async () =>
                {
                    await client.CreateDocumentOnSAAsync(new() { });
                };

        await act.Should().ThrowAsync<CisValidationException>();
    }

}
