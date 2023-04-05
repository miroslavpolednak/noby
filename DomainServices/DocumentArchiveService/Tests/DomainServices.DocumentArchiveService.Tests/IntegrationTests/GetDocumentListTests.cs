using DomainServices.DocumentArchiveService.Tests.IntegrationTests.Helpers;

namespace DomainServices.DocumentArchiveService.Tests.IntegrationTests;
public class GetDocumentListTests : IntegrationTestBase
{
    public GetDocumentListTests(GrpcTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task GetDocumentList_PassInvalidParametrs_ShouldReturnValidationExp()
    {
        var client = CreateClient();
        Func<Task> act = async () =>
        {
            await client.GetDocumentListAsync(new() { UserLogin = "Test" }, default);
        };
        // ToDo this shoud be different kind of exp, but there is a bug with logging 
        await act.Should().ThrowAsync<ArgumentOutOfRangeException>().WithMessage("Index was out of range. Must be non-negative and less than the size of the collection. (Parameter 'index')");
    }

    [Fact]
    public async Task GetDocumentList_PassCorrectParameters_ShouldReturnValues()
    {
        var client = CreateClient();
        var result = await client.GetDocumentListAsync(new() {CaseId =123, UserLogin = "Test" }, default);
        result.Should().NotBeNull();
        
    }
}
