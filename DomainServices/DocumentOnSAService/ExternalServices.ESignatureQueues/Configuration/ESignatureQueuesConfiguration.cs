using System.ComponentModel.DataAnnotations;
using CIS.Infrastructure.ExternalServicesHelpers;

namespace ExternalServices.ESignatureQueues.Configuration;

public class ESignatureQueuesConfiguration
{
    [Required]
    public string ConnectionString { get; set; } = null!;

    public ServiceImplementationTypes ImplementationType { get; set; } = ServiceImplementationTypes.Unknown;
}