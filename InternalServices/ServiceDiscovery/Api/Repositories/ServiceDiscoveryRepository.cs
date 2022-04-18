using Dapper;
using CIS.Infrastructure.Data;
using CIS.Core.Data;

namespace CIS.InternalServices.ServiceDiscovery.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal sealed class ServiceDiscoveryRepository 
    : DapperBaseRepository<ServiceDiscoveryRepository>
{
    public ServiceDiscoveryRepository(ILogger<ServiceDiscoveryRepository> logger, IConnectionProvider factory)
        : base(logger, factory)
    {
    }

    public async Task<List<Dto.ServiceModel>> GetList(Core.Types.ApplicationEnvironmentName environment, CancellationToken cancellationToken)
    {
        return await WithConnection(async c => (await c.QueryAsync<Dto.ServiceModel>("SELECT ServiceName, ServiceUrl, ServiceType FROM ServiceDiscovery WHERE EnvironmentName=@name", new { name = environment.ToString() }))
            .AsList(), cancellationToken);
    }
}
