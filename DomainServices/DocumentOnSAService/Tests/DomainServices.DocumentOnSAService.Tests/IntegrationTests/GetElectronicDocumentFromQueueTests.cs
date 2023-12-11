﻿using CIS.Core.Exceptions;
using CIS.Testing;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.DocumentOnSAService.Tests.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DomainServices.DocumentOnSAService.Tests.IntegrationTests;
public class GetElectronicDocumentFromQueueTests : IntegrationTestBase
{
    public GetElectronicDocumentFromQueueTests(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
    }

    [Fact]
    async Task GetElectronicDocumentFromQueue_NotExistDocumentOnSaId_ShouldReturnNotFoundException()
    {
        var client = CreateGrpcClient();

        Func<Task> act = async () =>
           {
               await client.GetElectronicDocumentFromQueueAsync(new()
               {
                   MainDocument = new MainDocument
                   {
                       //DocumentId =   
                   }
               });
           };

        await act.Should().ThrowAsync<CisNotFoundException>();
    }

    [Fact]
    async Task GetElectronicDocumentFromQueue_ExistDocumentOnSaId_ShouldReturnBinaryData()
    {
        var docOnSaEntity = CreateDocOnSaEntity();

        using var scope = Fixture.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();

        dbContext.DocumentOnSa.Add(docOnSaEntity);
        await dbContext.SaveChangesAsync();

        var client = CreateGrpcClient();
        var response = await client.GetElectronicDocumentFromQueueAsync(new()
        {
            MainDocument = new MainDocument
            {
                //DocumentOnSAId = docOnSaEntity.DocumentOnSAId  
            }
        });

        response.Should().NotBeNull();
        response.BinaryData.Should().NotBeNull();
        response.BinaryData.Length.Should().BeGreaterThan(0);
    }
}
