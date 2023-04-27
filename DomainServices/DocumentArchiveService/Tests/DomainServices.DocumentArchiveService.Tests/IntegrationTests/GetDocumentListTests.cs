using CIS.Core.Exceptions;
using CIS.Testing;
using DomainServices.DocumentArchiveService.Tests.IntegrationTests.Helpers;

namespace DomainServices.DocumentArchiveService.Tests.IntegrationTests;
public class GetDocumentListTests : IntegrationTestBase
{
    public GetDocumentListTests(WebApplicationFactoryFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task GetDocumentList_PassInvalidParametrs_ShouldReturnValidationExp()
    {
        var client = CreateGrpcClient();

        Func<Task> act = async () =>
        {
            await client.GetDocumentListAsync(new() { UserLogin = "Test" }, default);
        };

        await act.Should().ThrowAsync<CisValidationException>().WithMessage("One of main parameters have to be fill in (CaseId, PledgeAgreementNumber, ContractNumber, OrderId, AuthorUserLogin)");
    }

    [Fact]
    public async Task GetDocumentList_PassCorrectParameters_ShouldReturnValues()
    {
        var client = CreateGrpcClient();
        var result = await client.GetDocumentListAsync(new() { CaseId = 123, UserLogin = "Test" }, default);
        result.Should().NotBeNull();
        result.Metadata.Should().HaveCount(3);
    }
}
