using CIS.Core.Exceptions;
using CIS.Foms.Enums;
using CIS.Testing;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Tests.IntegrationTests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DomainServices.DocumentOnSAService.Tests.IntegrationTests;

public class SendDocumentPreviewTests : IntegrationTestBase
{
    public SendDocumentPreviewTests(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task SendDocumentPreview_ShouldFind_ExistingDocument()
    {
        var docOnSaEntity = CreateDocOnSaEntity(signatureTypeId: (int)SignatureTypes.Electronic);

        using var scope = Fixture.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();

        dbContext.DocumentOnSa.Add(docOnSaEntity);
        await dbContext.SaveChangesAsync();

        var client = CreateGrpcClient();
        await client.SendDocumentPreviewAsync(new() { DocumentOnSAId = docOnSaEntity.DocumentOnSAId });

        await dbContext.Entry(docOnSaEntity).ReloadAsync();
    }

    [Fact]
    public async Task SendDocumentPreview_ShouldNotFind_NonExistingDocument()
    {
        using var scope = Fixture.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();
        
        var client = CreateGrpcClient();

        await Assert.ThrowsAsync<CisNotFoundException>(async () =>
        {
            await client.SendDocumentPreviewAsync(new() { DocumentOnSAId = 99999 });
        });
    }
    
    /*[Fact]
    public async Task SendDocumentPreview_ShouldNotSendPreview_IfDocumentIsNotElectronic()
    {
        var docOnSaEntity = CreateDocOnSaEntity(signatureTypeId: (int)SignatureTypes.Biometric);

        using var scope = Fixture.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();

        dbContext.DocumentOnSa.Add(docOnSaEntity);
        await dbContext.SaveChangesAsync();

        var client = CreateGrpcClient();
        
        await Assert.ThrowsAsync<CisValidationException>(async () =>
        {
            await client.SendDocumentPreviewAsync(new() { DocumentOnSAId = docOnSaEntity.DocumentOnSAId });
        });
    }*/
}
