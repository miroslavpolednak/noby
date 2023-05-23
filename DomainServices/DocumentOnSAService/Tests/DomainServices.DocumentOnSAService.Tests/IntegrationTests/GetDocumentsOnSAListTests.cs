using CIS.Testing;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Tests.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DomainServices.DocumentOnSAService.Tests.IntegrationTests;
public class GetDocumentsOnSAListTests : IntegrationTestBase
{
    public GetDocumentsOnSAListTests(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task GetDocumentOnSaList_ShouldReturnCorrectCount()
    {
        var docOnSaEntity = CreateDocOnSaEntity(householdId: 1);

        using var scope = Fixture.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();

        dbContext.DocumentOnSa.AddRange(docOnSaEntity, CreateDocOnSaEntity(householdId: 2));
        await dbContext.SaveChangesAsync();

        var client = CreateGrpcClient();
        var response = await client.GetDocumentsOnSAListAsync(new() { SalesArrangementId = docOnSaEntity.SalesArrangementId });
        response.DocumentsOnSA.Should().HaveCount(2);
    }
}
