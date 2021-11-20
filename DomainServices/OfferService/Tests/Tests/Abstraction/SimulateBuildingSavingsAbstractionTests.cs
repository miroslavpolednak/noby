using CIS.Core.Results;
using CIS.Infrastructure.StartupExtensions;
using CIS.Testing;
using DomainServices.OfferService.Abstraction;

namespace DomainServices.OfferService.Tests;

public partial class SimulateBuildingSavingsAbstractionTests : BaseTest, IClassFixture<TestFixture<Program>>
{
    [Fact]
    public async Task simulation_returns_new_offerId()
    {
        var result = await _service.SimulateBuildingSavings(createRequest(100000));

        Assert.True(result.Success);
        Assert.True(((SuccessfulServiceCallResult<Contracts.SimulateBuildingSavingsResponse>)result).Model.OfferInstanceId > 0);
    }

    /*[Fact]
    public async Task simulation_queried_by_offerId_exists()
    {
        var result = await _service.SimulateHousingsSavings(createRequest(100000));

        
        var entity = _repository.GetByOfferInstanceId(result.OfferInstanceId);
        Assert.NotNull(entity);
    }*/

    [Fact]
    public async Task simulation_failes_due_to_invalid_target_amount()
    {
        var exception = await Assert.ThrowsAsync<CIS.Core.Exceptions.CisValidationException>(() => _service.SimulateBuildingSavings(createRequest(1)));

        Assert.Collection(exception.Errors, t => t.Key.EndsWith("10001"));
    }
}

public partial class SimulateBuildingSavingsAbstractionTests
{
    private readonly Api.Repositories.SimulateBuildingSavingsRepository _repository;
    private readonly IOfferServiceAbstraction _service;

    public SimulateBuildingSavingsAbstractionTests(TestFixture<Program> testFixture)
    {
        testFixture.Init(this)
            .ConfigureTestDatabase(options =>
            {
                options.SeedPaths = "~/CreateDbTables.sql";
            })
            .ConfigureTestServices(services =>
            {
                services
                    .AddTestServices(testFixture)
                    .AddCisEnvironmentConfiguration(opt =>
                    {
                        opt.DefaultApplicationKey = new(Constants.ApplicationKey);
                        opt.EnvironmentName = new(Constants.Environment);
                        opt.InternalServicesLogin = Constants.ServiceLogin;
                        opt.InternalServicePassword = Constants.ServicePassword;
                    })
                    .AddOfferServiceTest((provider, options) =>
                    {
                        options.ChannelOptionsActions.Add(t => t.HttpHandler = null);
                        options.ChannelOptionsActions.Add(t => t.HttpClient = testFixture.GrpcClient);
                        options.Address = testFixture.GrpcClient.BaseAddress;
                    });
            });

        _service = testFixture.GetService<IOfferServiceAbstraction>() ?? throw new ArgumentNullException();
        _repository = testFixture.GetService<Api.Repositories.SimulateBuildingSavingsRepository>() ?? throw new ArgumentNullException();
    }
}
