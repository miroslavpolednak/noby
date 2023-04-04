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
        var response = await client.GetDocumentListAsync(new() { UserLogin="Test"}, default);
        //ToDo
        //await action.Should().ThrowAsync<InvalidOperationException>();
    }
}
