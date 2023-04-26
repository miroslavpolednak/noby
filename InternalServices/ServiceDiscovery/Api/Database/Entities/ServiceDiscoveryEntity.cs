using CIS.InternalServices.ServiceDiscovery.Contracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIS.InternalServices.ServiceDiscovery.Api.Database.Entities;

[Table("ServiceDiscovery", Schema = "dbo")]
[PrimaryKey(nameof(EnvironmentName), nameof(ServiceName), nameof(ServiceType))]
internal sealed class ServiceDiscoveryEntity
{
    public string EnvironmentName { get; set; } = string.Empty;

    public string ServiceName { get; set; } = string.Empty;

    public string ServiceUrl { get; set; } = string.Empty;

    public byte ServiceType { get; set; }

    public bool AddToGlobalHealthCheck { get; set; }
}
