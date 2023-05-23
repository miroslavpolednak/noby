using CIS.Testing;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Tests.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DomainServices.DocumentOnSAService.Tests.IntegrationTests;
public class GetDocumentOnSaDataTests : IntegrationTestBase
{
    public GetDocumentOnSaDataTests(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task GetExistDocument_ShouldReturnExistDocument()
    {
        var docOnSaEntity = CreateDocOnSaEntity();

        using var scope = Fixture.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();

        dbContext.DocumentOnSa.Add(docOnSaEntity);
        await dbContext.SaveChangesAsync();

        var client = CreateGrpcClient();
        var response = await client.GetDocumentOnSADataAsync(new() { DocumentOnSAId = docOnSaEntity.DocumentOnSAId });
        response.Should().NotBeNull();
        response.DocumentTypeId.Should().Be(docOnSaEntity.DocumentTypeId);
        response.DocumentTemplateVersionId.Should().Be(docOnSaEntity.DocumentTemplateVersionId);
        response.Data.Should().Be(docOnSaEntity.Data);
        response.DocumentTemplateVariantId.Should().Be(response.DocumentTemplateVariantId);
    }
}
