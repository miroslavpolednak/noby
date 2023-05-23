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
public class GetDocumentsToSignListTests : IntegrationTestBase
{
    public GetDocumentsToSignListTests(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {

    }

    [Fact]
    public async Task GetDocumentToSignList_ProductRequest_ShouldReturnCorrectCount()
    {
        ArrangementServiceClient.GetSalesArrangement(0, Arg.Any<CancellationToken>())
           .ReturnsForAnyArgs(new SalesArrangement { SalesArrangementId = 1, CaseId = 2, SalesArrangementTypeId = 1 });

        // Simulate virtual entity (household without DoOnSa) 
        HouseholdServiceClient.GetHouseholdList(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new List<Household>
        {
            new Household {
                HouseholdId = 3,
                HouseholdTypeId = 1,
            }
        });

        var docOnSaEntity = CreateDocOnSaEntity(householdId: 1);

        using var scope = Fixture.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();

        dbContext.DocumentOnSa.AddRange(docOnSaEntity, CreateDocOnSaEntity(householdId: 2));
        await dbContext.SaveChangesAsync();

        var client = CreateGrpcClient();
        var response = await client.GetDocumentsToSignListAsync(new() { SalesArrangementId = docOnSaEntity.SalesArrangementId });
        response.DocumentsOnSAToSign.Should().HaveCount(3);
        // Virtual (without DocOnSa)
        response.DocumentsOnSAToSign.Any(r => string.IsNullOrWhiteSpace(r.FormId)).Should().BeTrue();
        // Real
        response.DocumentsOnSAToSign.Where(r => !string.IsNullOrWhiteSpace(r.FormId)).Should().HaveCount(2);

        // Simulate only entity (household with DoOnSa) 
        HouseholdServiceClient.GetHouseholdList(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new List<Household>
        {
            new Household {
                HouseholdId = 1,
                HouseholdTypeId = 1,
            }
        });

        var responseRealOnly = await client.GetDocumentsToSignListAsync(new() { SalesArrangementId = docOnSaEntity.SalesArrangementId });
        responseRealOnly.DocumentsOnSAToSign.Should().HaveCount(2);

        // Real only
        responseRealOnly.DocumentsOnSAToSign.Where(r => !string.IsNullOrWhiteSpace(r.FormId)).Should().HaveCount(2);
    }
}


public class GetDocumentsToSignListTestsPart2 : IntegrationTestBase
{
    public GetDocumentsToSignListTestsPart2(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task GetDocumentToSignList_ServiceRequest_ShouldReturnCorrectCount()
    {
        ArrangementServiceClient.GetSalesArrangement(0, Arg.Any<CancellationToken>())
             .ReturnsForAnyArgs(new SalesArrangement { SalesArrangementId = 2, CaseId = 2, SalesArrangementTypeId = 6 }); //One of service type

        var client = CreateGrpcClient();
        var response = await client.GetDocumentsToSignListAsync(new() { SalesArrangementId = 2 });
        response.DocumentsOnSAToSign.Should().HaveCount(1);
    }
}
