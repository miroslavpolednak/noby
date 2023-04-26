using CIS.InternalServices.ServiceDiscovery.Contracts;
using CIS.Testing;
using Microsoft.Extensions.DependencyInjection;
using static CIS.InternalServices.ServiceDiscovery.Contracts.v1.DiscoveryService;

namespace CIS.InternalServices.ServiceDiscovery.Tests.IntegrationTests.Helpers;

public abstract class IntegrationTestBase 
    : IClassFixture<WebApplicationFactoryFixture<Program>>
{
    public IntegrationTestBase(WebApplicationFactoryFixture<Program> fixture)
    {
        Fixture = fixture;
    }

    protected WebApplicationFactoryFixture<Program> Fixture { get; }

    protected DiscoveryServiceClient CreateGrpcClient()
    {
        return Fixture.CreateGrpcClient<DiscoveryServiceClient>();
    }

    protected void SeedDatabaseWithServices()
    {
        using var scope = Fixture.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<Api.Database.ServiceDiscoveryDbContext>();
        
        // pokud je jiza naseedovano, tak nic nedelej
        if (dbContext.ServiceDiscoveryEntities.Any())
        {
            return;
        }

        List<Api.Database.Entities.ServiceDiscoveryEntity> data = new()
        {
            create(1, 1),
            create(1, 2),
            create(1, 1, ServiceTypes.Proprietary),
            create(2, 1),
            create(2, 2),
        };

        dbContext.ServiceDiscoveryEntities.AddRange(data);
        dbContext.SaveChanges();

        Api.Database.Entities.ServiceDiscoveryEntity create(int environment, int index, ServiceTypes serviceType = ServiceTypes.Grpc)
        {
            return new() { 
                EnvironmentName = environment == 1 ? Constants.ServicesEnvironmentName1 : Constants.ServicesEnvironmentName2, 
                ServiceName = $"DS:Service{index}", 
                ServiceUrl = $"http://0.0.0.0:{index}",
                ServiceType = (byte)serviceType
            };
        }
    }
}
