using CIS.Testing;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CIS.Core.Types;
using CIS.Infrastructure.Caching;

namespace CIS.InternalServices.ServiceDiscovery.Tests;

public abstract partial class BaseDiscoveryServiceAbstractionTest
{
    [Fact]
    public async Task GetServices_ShouldReturnAll()
    {
        // act
        var result = await _service.GetServices(new ApplicationEnvironmentName(Constants.Environment));

        // assert
        Assert.True(result.Any());
    }

    [Fact]
    public async Task GetServices_InvalidEnvironment_ShouldThrowException()
    {
        // act, assert
        var exception = await Assert.ThrowsAsync<Core.Exceptions.CisNotFoundException>(async () => await _service.GetServices(new ApplicationEnvironmentName("invalidenv")));
    }

    [Fact]
    public async Task GetService_ShouldReturnService()
    {
        var name = new ServiceName(ServiceName.WellKnownServices.Redis);
        var result = await _service.GetService(new ApplicationEnvironmentName(Constants.Environment), name, Contracts.ServiceTypes.Grpc);

        Assert.Equal(name.ToString(), result.ServiceName);
    }

    [Fact]
    public async Task GetService_InvalidName_ShouldThrowException()
    {
        await Assert.ThrowsAsync<Core.Exceptions.CisNotFoundException>(async () => await _service.GetService(new ApplicationEnvironmentName(Constants.Environment), new("wrongname"), Contracts.ServiceTypes.Grpc));
    }
}

public abstract partial class BaseDiscoveryServiceAbstractionTest
{
    protected readonly TestFixture<Program> _testFixture;
    protected readonly IDiscoveryServiceAbstraction _service;

    public BaseDiscoveryServiceAbstractionTest(TestFixture<Program> testFixture)
    {
        Api.AppConfiguration appConfiguration = new();

        _testFixture = testFixture
            .Recreate()
            .Init(this)
            .ConfigureTestDatabase(options =>
            {
                options.SeedPaths = "~/DiscoveryServiceDatabaseSeed.sql";
            })
            .ConfigureAppConfiguration((context, builder) =>
            {
                var c = builder.Build();
                c.GetSection("AppConfiguration").Bind(appConfiguration);
            })
            .ConfigureTestServices(services =>
            {
                services.AddHttpContextAccessor();

                services.AddCisServiceDiscoveryTest(options =>
                {
                    options.ChannelOptionsActions.Add(t => t.HttpHandler = null);
                    options.ChannelOptionsActions.Add(t => t.HttpClient = testFixture.GrpcClient);
                    options.Address = testFixture.GrpcClient.BaseAddress;
                });

                // abstraction caching
                var svcToRemove = services
                    .Where(t => t.Lifetime == ServiceLifetime.Singleton && t.ServiceType.IsGenericType && t.ServiceType.ToString().Contains("IGlobalCache"))
                    .FirstOrDefault(t => t.ServiceType.GenericTypeArguments[0] == typeof(DiscoveryService));
                if (svcToRemove is not null) services.Remove(svcToRemove);
                    
                // in memory
                switch (appConfiguration.CacheType)
                {
                    case CacheTypes.InMemory:
                        services.AddSingleton<IGlobalCache<DiscoveryService>>(new Infrastructure.Caching.InMemory.InMemoryGlobalCache<DiscoveryService>(""));
                        break;
                    case CacheTypes.Redis:
                        services.AddSingleton<IGlobalCache<DiscoveryService>>(new Infrastructure.Caching.Redis.RedisGlobalCache<DiscoveryService>(appConfiguration.CacheConnectionString ?? throw new ArgumentNullException("connectionstring"), "Development:ServiceDiscovery:"));
                        break;
                }
            });

        _service = _testFixture.GetService<IDiscoveryServiceAbstraction>() ?? throw new ArgumentNullException("_service");
    }
}
