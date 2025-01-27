﻿using SharedTypes.Enums;
using CIS.Testing;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Tests.IntegrationTests.Helpers;
using DomainServices.ProductService.Contracts;
using DomainServices.SalesArrangementService.Contracts;
using FastEnumUtility;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace DomainServices.DocumentOnSAService.Tests.IntegrationTests;

public class SignDocumentTests : IntegrationTestBase
{
    public SignDocumentTests(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
        // Service request No Saml call
        ArrangementServiceClient.GetSalesArrangement(0, Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(new SalesArrangement { SalesArrangementId = 2, CaseId = 2, SalesArrangementTypeId = 6, State = 7 }); //One of service type

        ProductServiceClient.GetMortgage(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns(new GetMortgageResponse
        {
            Mortgage = new MortgageData
            {
                ProductTypeId = 4
            }
        });
    }

    [Fact]
    public async Task SignDocumentManually_ShouldSucces()
    {
        var docOnSaEntity = CreateDocOnSaEntity(householdId: 1);

        using var scope = Fixture.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();
        dbContext.DocumentOnSa.AddRange(docOnSaEntity);

        await dbContext.SaveChangesAsync();

        var client = CreateGrpcClient();

        await client.SignDocumentAsync(new() { DocumentOnSAId = docOnSaEntity.DocumentOnSAId, SignatureTypeId = SignatureTypes.Paper.ToByte() });

        // Check
        var signEntity = await dbContext.DocumentOnSa.SingleAsync(r => r.DocumentOnSAId == docOnSaEntity.DocumentOnSAId);
        await dbContext.Entry(signEntity).ReloadAsync();
        signEntity.Should().NotBeNull();
        signEntity.IsSigned.Should().BeTrue();
        signEntity.SignatureDateTime.Should().NotBeNull();
        signEntity.SignatureConfirmedBy.Should().NotBeNull();
    }
}
