using CIS.Core.Exceptions;
using CIS.Testing;
using DomainServices.DocumentArchiveService.Api;
using DomainServices.DocumentArchiveService.Api.Database;
using DomainServices.DocumentArchiveService.Tests.IntegrationTests.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.DocumentArchiveService.Tests.IntegrationTests;

public class SetDocumentStatusInQueueTests : IntegrationTestBase
{
    public SetDocumentStatusInQueueTests(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task TrySetCorrectStateInQueue_NotExistEntity_ShouldReturn_NotFoundExp()
    {
        var client = CreateGrpcClient();

        Func<Task> act = async () =>
        {
            await client.SetDocumentStatusInQueueAsync(new() { EArchivId = "NotExist", StatusInQueue = 302 }, default);
        };

        await act.Should().ThrowAsync<CisNotFoundException>().WithMessage(ErrorCodeMapper.GetMessage(ErrorCodeMapper.DocumentWithEArchiveIdNotExist));
    }

    [Fact]
    public async Task TrySetInCorrectStateInQueue_ExistEntity_ShouldReturnValidationExp()
    {
        var client = CreateGrpcClient();

        Func<Task> act = async () =>
        {
            await client.SetDocumentStatusInQueueAsync(new() { EArchivId = "SomeId", StatusInQueue = 500000 }, default);
        };

        await act.Should().ThrowAsync<CisValidationException>().WithMessage(ErrorCodeMapper.GetMessage(ErrorCodeMapper.StateInQueueNotAllowed));

    }

    [Fact]
    public async Task TrySetCorrectStateInQueue_ExistEntity_StateShouldBeSetCorrectly()
    {
        var scope = Fixture.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DocumentArchiveDbContext>();

        var documentinterface = CreateDocumentInterfaceEntity();
        await db.DocumentInterface.AddAsync(documentinterface);
        await db.SaveChangesAsync();

        var client = CreateGrpcClient();
        await client.SetDocumentStatusInQueueAsync(new() { EArchivId = documentinterface.DocumentId, StatusInQueue = 302 });

        var entity = db.DocumentInterface.First(r => r.DocumentId == documentinterface.DocumentId);
        await db.Entry(entity).ReloadAsync();
        entity!.Status.Should().Be(302);
    }
}
