using System.ComponentModel.DataAnnotations;
using CIS.Foms.Enums;

namespace ExternalServices.ESignatureQueues.Configuration;

public class ESignatureQueuesConfiguration
{
    [Required]
    public string ConnectionString { get; set; } = null!;

    public ServiceImplementationTypes ImplementationType { get; set; } = ServiceImplementationTypes.Unknown;
}