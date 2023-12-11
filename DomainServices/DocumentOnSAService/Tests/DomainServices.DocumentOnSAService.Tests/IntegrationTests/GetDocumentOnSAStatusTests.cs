using CIS.Testing;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Tests.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DomainServices.DocumentOnSAService.Tests.IntegrationTests;
public class GetDocumentOnSAStatusTests(WebApplicationFactoryFixture<Program> fixture) : IntegrationTestBase(fixture)
{
    [Fact]
    public async Task GetDocumentOnSaStatus_Should_ReturnCorrectValues()
    {
        var docOnSaEntity = CreateDocOnSaEntity();

        using var scope = Fixture.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();

        dbContext.DocumentOnSa.Add(docOnSaEntity);
        await dbContext.SaveChangesAsync();

        var client = CreateGrpcClient();
        var response = await client.GetDocumentOnSAStatusAsync(new() { DocumentOnSAId = docOnSaEntity.DocumentOnSAId, SalesArrangementId = docOnSaEntity.SalesArrangementId });
        response.Should().NotBeNull();
        response.DocumentOnSAId.Should().Be(docOnSaEntity.DocumentOnSAId);
        response.Source.Should().HaveSameValueAs(docOnSaEntity.Source);
        response.IsValid.Should().Be(docOnSaEntity.IsValid);
        response.IsSigned.Should().Be(docOnSaEntity.IsSigned);
        response.EArchivIdsLinked.Should().HaveCount(1);
    }
}
