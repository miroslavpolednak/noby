using CIS.Testing;
using CIS.Testing.Database;
using DomainServices.UserService.Clients.Services;
using DomainServices.UserService.Clients;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.DocumentOnSAService.Api.Database;
using static DomainServices.DocumentOnSAService.Contracts.v1.DocumentOnSAService;
using CIS.InternalServices.DataAggregatorService.Clients;
using DomainServices.SalesArrangementService.Clients;
using NSubstitute;

namespace DomainServices.DocumentOnSAService.Tests.IntegrationTests.Helpers;

public abstract class IntegrationTestBase : IClassFixture<WebApplicationFactoryFixture<Program>>
{
    //Mocks
    internal IDataAggregatorServiceClient DataAggregatorServiceClient { get; }
    internal ISalesArrangementServiceClient ArrangementServiceClient { get; }

    public IntegrationTestBase(WebApplicationFactoryFixture<Program> fixture)
    {
        Fixture = fixture;

        DataAggregatorServiceClient = Substitute.For<IDataAggregatorServiceClient>();

        ArrangementServiceClient = Substitute.For<ISalesArrangementServiceClient>();

        ConfigureWebHost();

        PrepareDatabase();
    }
    public WebApplicationFactoryFixture<Program> Fixture { get; }

    private void ConfigureWebHost()
    {
        Fixture.ConfigureCisTestOptions(options =>
        {
            // Need real db, for sequence testing
            options.DbMockAdapter = new SqliteInMemoryMockAdapter();
        })
        .ConfigureServices(services =>
        {
            // This mock is necessary for mock of service discovery
            services.RemoveAll<IUserServiceClient>().AddSingleton<IUserServiceClient, MockUserService>();

            // NSubstitute mocks
            services.RemoveAll<IDataAggregatorServiceClient>().AddSingleton(DataAggregatorServiceClient);
            services.RemoveAll<ISalesArrangementServiceClient>().AddSingleton(ArrangementServiceClient);
        });
    }

    protected DocumentOnSAServiceClient CreateGrpcClient()
    {
        return Fixture.CreateGrpcClient<DocumentOnSAServiceClient>(true);
    }

    protected void PrepareDatabase()
    {
        var scope = Fixture.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();
        context.Database.EnsureCreated();
    }
}
