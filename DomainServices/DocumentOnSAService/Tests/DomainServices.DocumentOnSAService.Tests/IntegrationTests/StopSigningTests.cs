using CIS.InternalServices.DataAggregatorService.Contracts;
using CIS.Testing;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Tests.IntegrationTests.Helpers;
using DomainServices.HouseholdService.Contracts;
using DomainServices.SalesArrangementService.Contracts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace DomainServices.DocumentOnSAService.Tests.IntegrationTests;

public class StopSigningTests : IntegrationTestBase
{
    public StopSigningTests(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
        //Mocks default
        ArrangementServiceClient.GetSalesArrangement(0, Arg.Any<CancellationToken>()).ReturnsForAnyArgs(new SalesArrangement { SalesArrangementId = 1, CaseId = 2, State = 7 });

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

        DocumentArchiveServiceClient.GenerateDocumentId(new()).ReturnsForAnyArgs("KBHXXD00000000000000000000351");
    }

    [Fact]
    public async Task StopSigning_ExistDocOnSa_ShouldSetIsSignedToFalse()
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

        await client.StopSigningAsync(new() { DocumentOnSAId = response.DocumentOnSa.DocumentOnSAId });

        using var scope = Fixture.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();
        var entityAfterCall = await dbContext.DocumentOnSa.FirstAsync(r => r.DocumentOnSAId == response.DocumentOnSa.DocumentOnSAId);
        entityAfterCall!.IsValid.Should().BeFalse();
    }

}
