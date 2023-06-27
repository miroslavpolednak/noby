using CIS.Testing;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Tests.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DomainServices.DocumentOnSAService.Tests.IntegrationTests;
public class GetElectronicDocumentPreviewTests : IntegrationTestBase
{
    public GetElectronicDocumentPreviewTests(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
    }

    [Fact]
    async Task GetElectronicDocumentPreview_ShouldReturnDocument()
    {
        var docOnSaEntity = CreateDocOnSaEntity();

        using var scope = Fixture.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();

        dbContext.DocumentOnSa.Add(docOnSaEntity);
        await dbContext.SaveChangesAsync();

        var client = CreateGrpcClient();
        var response = await client.GetElectronicDocumentPreviewAsync(new()
        {
            DocumentOnSAId = docOnSaEntity.DocumentOnSAId
        });

        response.Should().NotBeNull();
        response.BinaryData.Should().NotBeNull();
        response.BinaryData.Length.Should().BeGreaterThan(0);
    }
}
