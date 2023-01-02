using CIS.Foms.Enums;
using System.ComponentModel.DataAnnotations;

namespace DomainServices.DocumentArchiveService.Api.ExternalServices.Tcp.Configuration;

public class TcpConfiguration
{
    [Required]
    public string Connectionstring { get; set; } = null!;

    public ServiceImplementationTypes ImplementationType { get; set; } = ServiceImplementationTypes.Unknown;
}
