using CIS.Testing;
using CIS.Testing.Database;
using DomainServices.UserService.Clients.Services;
using DomainServices.UserService.Clients;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.DocumentOnSAService.Api.Database;
using static DomainServices.DocumentArchiveService.Contracts.v1.DocumentArchiveService;
using static DomainServices.DocumentOnSAService.Contracts.v1.DocumentOnSAService;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Tests.IntegrationTests.Helpers;

public abstract class IntegrationTestBase : IClassFixture<WebApplicationFactoryFixture<Program>>
{
    public IntegrationTestBase(WebApplicationFactoryFixture<Program> fixture)
    {
        Fixture = fixture;

        ConfigureWebHost();

        PrepareDatabase();
    }
    public WebApplicationFactoryFixture<Program> Fixture { get; }

    private void ConfigureWebHost()
    {
        Fixture.ConfigureCisTestOptions(options =>
        {
            options.DbMockAdapter = new SqliteInMemoryMockAdapter();
        })
        .ConfigureServices(services =>
        {
            // This mock is necessary for mock of service discovery
            services.RemoveAll<IUserServiceClient>().AddSingleton<IUserServiceClient, MockUserService>();
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
