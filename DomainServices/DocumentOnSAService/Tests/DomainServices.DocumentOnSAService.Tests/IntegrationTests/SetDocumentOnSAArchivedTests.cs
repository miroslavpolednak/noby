using CIS.Foms.Enums;
using CIS.Testing;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Tests.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DomainServices.DocumentOnSAService.Tests.IntegrationTests;

public class SetDocumentOnSAArchivedTests : IntegrationTestBase
{
    public SetDocumentOnSAArchivedTests(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task SetDocumentOnSAArchived_ShouldSetIsArchived_Correctly()
    {
        var docOnSaEntity = CreateDocOnSaEntity(signatureTypeId: (int)SignatureTypes.Electronic);

        using var scope = Fixture.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();

        dbContext.DocumentOnSa.Add(docOnSaEntity);
        await dbContext.SaveChangesAsync();

        var client = CreateGrpcClient();
        await client.SetDocumentOnSAArchivedAsync(new() { DocumentOnSAId = docOnSaEntity.DocumentOnSAId });

        await dbContext.Entry(docOnSaEntity).ReloadAsync();
        docOnSaEntity.IsArchived.Should().BeTrue();
    }
}
