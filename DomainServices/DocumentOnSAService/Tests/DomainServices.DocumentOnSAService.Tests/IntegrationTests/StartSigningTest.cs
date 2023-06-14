using CIS.Core.Exceptions;
using CIS.InternalServices.DataAggregatorService.Contracts;
using CIS.Testing;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Tests.IntegrationTests.Helpers;
using DomainServices.HouseholdService.Contracts;
using DomainServices.SalesArrangementService.Contracts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace DomainServices.DocumentOnSAService.Tests.IntegrationTests;

public class StartSigningTest : IntegrationTestBase
{
    private string eArchiveId = "KBHXXD00000000000000000000351";

    public StartSigningTest(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
        //Mocks default
        ArrangementServiceClient.GetSalesArrangement(0, Arg.Any<CancellationToken>()).ReturnsForAnyArgs(new SalesArrangement { SalesArrangementId = 1, State = 1, CaseId = 2 });

        var resp = new GetDocumentDataResponse
        {
            DocumentTemplateVariantId = 1,
            DocumentTemplateVersionId = 4,
        };

        resp.DocumentData.Add(new DocumentFieldData { FieldName = "TestName", Text = "TestText" });
        DataAggregatorServiceClient.GetDocumentData(Arg.Any<GetDocumentDataRequest>()).ReturnsForAnyArgs(resp);

        HouseholdServiceClient.GetHouseholdList(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new List<Household>
        {
            new Household {
                HouseholdId = 1,
                HouseholdTypeId = 1,
            }
        });

        DocumentArchiveServiceClient.GenerateDocumentId(new()).ReturnsForAnyArgs(eArchiveId);
    }

    [Fact]
    public async Task StartSigning_ShouldCreate_DocOnSa_ShouldReturnCorrectData()
    {
        //Grpc call
        var client = CreateGrpcClient();

        var salesArrangementId = 1;
        var documentTypeId = 4;
        var signatureMethodCode = "PHYSICAL";

        var response = await client.StartSigningAsync(new()
        {
            SalesArrangementId = salesArrangementId,
            DocumentTypeId = documentTypeId,
            SignatureMethodCode = signatureMethodCode
        });

        // response check
        response.Should().NotBeNull();
        response.DocumentOnSa.HouseholdId.Should().Be(1);
        response.DocumentOnSa.DocumentOnSAId.Should().Be(1);
        response.DocumentOnSa.SignatureMethodCode.Should().Be(signatureMethodCode);
        response.DocumentOnSa.DocumentTypeId.Should().Be(documentTypeId);
        response.DocumentOnSa.EArchivId.Should().Be(eArchiveId);
        response.DocumentOnSa.FormId.Should().Be("N00000000000101");
        response.DocumentOnSa.IsValid.Should().BeTrue();

        using var scope = Fixture.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();

        var docOnSaEntity = dbContext.DocumentOnSa.SingleOrDefault(r => r.SalesArrangementId == salesArrangementId);
        docOnSaEntity.Should().NotBeNull();
        docOnSaEntity!.HouseholdId.Should().Be(1);
        docOnSaEntity!.DocumentOnSAId.Should().Be(1);
        docOnSaEntity!.SignatureMethodCode.Should().Be(signatureMethodCode);
        docOnSaEntity!.DocumentTypeId.Should().Be(documentTypeId);
        docOnSaEntity!.EArchivId.Should().Be(eArchiveId);
        docOnSaEntity!.FormId.Should().Be("N00000000000101");
        docOnSaEntity!.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task StartSigning_PassInvalidSignatureMethod_ShouldReturnValidationExp()
    {
        //Grpc call
        var client = CreateGrpcClient();

        var salesArrangementId = 1;
        var documentTypeId = 4;
        var signatureMethodCode = "NotExistMethod";

        Func<Task> act = async () =>
                {
                    var response = await client.StartSigningAsync(new()
                    {
                        SalesArrangementId = salesArrangementId,
                        DocumentTypeId = documentTypeId,
                        SignatureMethodCode = signatureMethodCode
                    });
                };

        await act.Should().ThrowAsync<CisNotFoundException>().WithMessage($"SignatureMethod {signatureMethodCode} does not exist.");
    }

    [Fact]
    public async Task StartSigning_PassInvalidParameters_ShouldReturnValidationExp_WithCorrectMessage()
    {
        //Grpc call
        var client = CreateGrpcClient();

        var documentTypeId = 4;
        var signatureMethodCode = "NotExistMethod";

        Func<Task> act = async () =>
        {
            var response = await client.StartSigningAsync(new()
            {
                DocumentTypeId = documentTypeId,
                SignatureMethodCode = signatureMethodCode
            });
        };

        await act.Should().ThrowAsync<CisValidationException>().WithMessage($"SalesArrangementId is required");
    }

}
