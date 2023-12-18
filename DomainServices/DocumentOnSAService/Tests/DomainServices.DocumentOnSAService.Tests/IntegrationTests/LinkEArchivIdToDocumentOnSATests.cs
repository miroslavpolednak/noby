using CIS.Core.Exceptions;
using CIS.Testing;
using DomainServices.DocumentOnSAService.Api;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Tests.IntegrationTests.Helpers;
using DomainServices.SalesArrangementService.Contracts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace DomainServices.DocumentOnSAService.Tests.IntegrationTests;

public class LinkEArchivIdToDocumentOnSATests : IntegrationTestBase
{
    public LinkEArchivIdToDocumentOnSATests(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
        // Mocks
        ArrangementServiceClient.GetSalesArrangement(Arg.Any<int>(), Arg.Any<CancellationToken>())
           .ReturnsForAnyArgs(new SalesArrangement { SalesArrangementId = 1, CaseId = 2, SalesArrangementTypeId = 1, State = 1 });
    }

    [Fact]
    public async Task LinkEArchivIdToDocumentOnSA_PassInvalidParams_ShouldReturnValidationExp()
    {
        var client = CreateGrpcClient();
        Func<Task> act = async () =>
        {
            await client.LinkEArchivIdToDocumentOnSAAsync(new() { DocumentOnSAId = 1 });
        };

        await act.Should().ThrowAsync<CisValidationException>().WithMessage(ErrorCodeMapper.GetMessage(ErrorCodeMapper.EArchivIdIsRequired));
    }

    [Fact]
    public async Task LinkEArchivIdToDocumentOnSA_PassValidParams_ShouldLinkEarchiveIdCorrectly()
    {
       

        var docOnSaEntity = CreateDocOnSaEntity();

        using var scope = Fixture.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();

        dbContext.DocumentOnSa.Add(docOnSaEntity);
        await dbContext.SaveChangesAsync();

        var client = CreateGrpcClient();
        var eArchivId = "KBHXXD00000000000000000000351";
        await client.LinkEArchivIdToDocumentOnSAAsync(new() { DocumentOnSAId = docOnSaEntity.DocumentOnSAId, EArchivId = eArchivId });

        var eArchiveIdsLinked = dbContext.EArchivIdsLinked.First(r => r.DocumentOnSAId == docOnSaEntity.DocumentOnSAId);
        eArchiveIdsLinked.EArchivId.Should().Be(eArchivId);
    }
}
