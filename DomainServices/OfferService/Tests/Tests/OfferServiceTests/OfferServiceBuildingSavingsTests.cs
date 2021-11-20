using CIS.Testing;
using Grpc.Core;

namespace DomainServices.OfferService.Tests;

public partial class OfferServiceBuildingSavingsTests : BaseTest, IClassFixture<TestFixture<Program>>
{
    [Fact]
    public async Task CorrectInputs_CheckResult_IdShouldBeSet()
    {
        // act
        var result = await _service.SimulateBuildingSavingsAsync(createRequest(100000));

        // assert
        Assert.True(result.OfferInstanceId > 0);
    }

    [Fact]
    public async Task Simulate_CheckResultsInDatabase_InstanceExists()
    {
        // act
        var result = await _service.SimulateBuildingSavingsAsync(createRequest(100000));

        // assert
        var entity = _repository.Get(result.OfferInstanceId);
        Assert.NotNull(entity);
    }

    [Fact]
    public async Task Simulate_CheckResultsInDatabase_InputDataShouldBeEqual()
    {
        var sut = createRequest(100000, productCode: 9);

        // act
        var result = await _service.SimulateBuildingSavingsAsync(sut);

        // pull data from DB
        var entity = await _repository.Get(result.OfferInstanceId);
            
        // assert
        Assert.Equal(sut.InputData.TargetAmount, entity?.InputData.TargetAmount);
        Assert.Equal(sut.InputData.ClientIsNaturalPerson, entity?.InputData.ClientIsNaturalPerson);
        Assert.Equal(sut.InputData.ClientIsSVJ, entity?.InputData.ClientIsSVJ);
        Assert.Equal(sut.InputData.ProductCode, entity?.InputData.ProductCode);
        Assert.Equal(sut.InputData.ActionCode, entity?.InputData.ActionCode);
    }

    [Fact]
    public async Task Simulate_CheckResultsInDatabase_OutputDataShouldBeEqual()
    {
        var sut = createRequest(100000);

        // act
        var result = await _service.SimulateBuildingSavingsAsync(sut);

        // pull data from DB
        var entity = await _repository.Get(result.OfferInstanceId);

        // assert
        Assert.Equal(result.BuildingSavings.BenefitInterests, entity?.BuildingSavings.BenefitInterests);
        Assert.Equal(result.BuildingSavings.ContractTerminationDate, entity?.BuildingSavings.ContractTerminationDate);
        Assert.Equal(result.BuildingSavings.ContractTerminationDate, entity?.BuildingSavings.ContractTerminationDate);
        Assert.Equal(result.BuildingSavings.DepositBalance, entity?.BuildingSavings.DepositBalance);
        Assert.Equal(result.BuildingSavings.TotalFees, entity?.BuildingSavings.TotalFees);
        Assert.Equal(result.BuildingSavings.TotalDeposits, entity?.BuildingSavings.TotalDeposits);
        Assert.Equal(result.BuildingSavings.TotalGovernmentIncentives, entity?.BuildingSavings.TotalGovernmentIncentives);
        Assert.Equal(result.BuildingSavings.TotalInterests, entity?.BuildingSavings.TotalInterests);
        Assert.Equal(result.BuildingSavings.TotalSaved, entity?.BuildingSavings.TotalSaved);
    }

    [Fact]
    public async Task InvalidTargetAmount_ShouldThrowException()
    {
        // act
        var exception = await Assert.ThrowsAsync<RpcException>(() => _service.SimulateBuildingSavingsAsync(createRequest(1)).ResponseAsync);

        // assert
        Assert.True(exception.StatusCode == StatusCode.FailedPrecondition);
        Assert.Equal("10001", exception.Trailers.FirstOrDefault(t => t.Key == "ciscode")?.Value ?? "");
    }
}

public partial class OfferServiceBuildingSavingsTests
{
    private readonly Contracts.v1.OfferService.OfferServiceClient _service;
    private readonly Api.Repositories.SimulateBuildingSavingsRepository _repository;

    public OfferServiceBuildingSavingsTests(TestFixture<Program> testFixture)
    {
        testFixture
            //.Recreate()
            .Init(this)
            .ConfigureTestDatabase(options =>
            {
                options.SeedPaths = "~/CreateDbTables.sql";
            })
            .ConfigureTestServices(services =>
            {
                services.AddTestServices(testFixture);
            });

        _service = new Contracts.v1.OfferService.OfferServiceClient(testFixture.GetGrpcChannel());
        _repository = testFixture.GetService<Api.Repositories.SimulateBuildingSavingsRepository>() ?? throw new ArgumentNullException();
    }
}
