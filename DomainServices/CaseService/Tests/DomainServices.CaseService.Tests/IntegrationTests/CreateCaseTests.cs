using CIS.Testing;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace DomainServices.CaseService.Tests.IntegrationTests;

public class CreateCaseTests
    : IntegrationTestBase
{
    [Fact]
    public async Task RequestValidationCaseOwnerUserId_ShouldThrowRpcException()
    {
        var request = createFullRequest();
        request.CaseOwnerUserId = 0;
        Func<Task> act = async () => await CreateGrpcClient().CreateCaseAsync(request);
        await act.Should().ThrowAsync<RpcException>().Where(t => t.StatusCode == StatusCode.InvalidArgument);
    }

    [Fact]
    public async Task RequestValidationDataMissing_ShouldThrowRpcException()
    {
        var request = createFullRequest();
        request.Data = null;
        Func<Task> act = async () => await CreateGrpcClient().CreateCaseAsync(request);
        await act.Should().ThrowAsync<RpcException>().Where(t => t.StatusCode == StatusCode.InvalidArgument);
    }

    [Fact]
    public async Task GetContractNumberFromEasFail_ShouldThrowRpcException()
    {
        MockEas
            .Setup(t => t.GetCaseId(It.IsAny<CIS.Foms.Enums.IdentitySchemes>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new CIS.Core.Exceptions.CisValidationException(9102, "test_exception"));

        var request = createFullRequest();
        Func<Task> act = async () => await CreateGrpcClient().CreateCaseAsync(request);

        await act.Should().ThrowAsync<RpcException>().Where(t => t.StatusCode == StatusCode.InvalidArgument);
    }

    public async Task x()
    {
        var request = createFullRequest();
        var result = await CreateGrpcClient().CreateCaseAsync(request);
        
        result.CaseId.Should().BeGreaterThan(0);

        var dbContext = Fixture.Services.GetRequiredService<CaseService.Api.Database.CaseServiceDbContext>();
        var caseInstance = dbContext.Cases.First(t => t.CaseId == result.CaseId);

        caseInstance.ProductTypeId.Should().Be(request.Data.ProductTypeId);
        caseInstance.FirstNameNaturalPerson.Should().Be(request.Customer.FirstNameNaturalPerson);
        caseInstance.Name.Should().Be(request.Customer.Name);
        caseInstance.OwnerUserId.Should().Be(request.CaseOwnerUserId);
    }

    private Contracts.CreateCaseRequest createFullRequest()
        => new Contracts.CreateCaseRequest
        {
            Customer = new()
            {
                Name = "Doe",
                FirstNameNaturalPerson = "John",
                Cin = "1234",
                DateOfBirthNaturalPerson = new DateTime(2000, 1, 1)
            },
            Data = new()
            {
                ProductTypeId = 1,
                TargetAmount = 100000
            },
            CaseOwnerUserId = UserService.Clients.Services.MockUserService.DefaultUserId
        };

    public CreateCaseTests(WebApplicationFactoryFixture<Program> fixture)
        : base(fixture)
    {
    }
}
