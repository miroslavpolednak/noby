using CIS.InternalServices.ServiceDiscovery.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.ServiceDiscovery.Api.Database;

internal sealed class ServiceDiscoveryDbContext
    : DbContext
{
    public ServiceDiscoveryDbContext(DbContextOptions<ServiceDiscoveryDbContext> options)
        : base(options) { }

    public DbSet<ServiceDiscoveryEntity> ServiceDiscoveryEntities { get; set; }
}
