using CIS.Foms.Enums;
using CIS.Testing;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Tests.IntegrationTests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace DomainServices.DocumentOnSAService.Tests.IntegrationTests;

public class RefreshElectronicDocumentTests : IntegrationTestBase
{
    public RefreshElectronicDocumentTests(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
        ESignaturesClient.GetDocumentStatus(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(EDocumentStatuses.NEW);
    }

    [Fact]
    public async Task RefreshElectronicDocument_NewStatusReturnedFromESignature_ShouldDoNothing_ShouldReturnExp()
    {
        var docOnSaEntity = CreateDocOnSaEntity();

        using var scope = Fixture.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();
        dbContext.DocumentOnSa.Add(docOnSaEntity);
        await dbContext.SaveChangesAsync();

        var client = CreateGrpcClient();

        var response = await client.RefreshElectronicDocumentAsync(new() { DocumentOnSAId = 1 }, default);
    }
}
