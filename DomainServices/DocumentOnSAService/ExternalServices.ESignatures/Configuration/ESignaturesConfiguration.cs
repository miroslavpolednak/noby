using System.ComponentModel.DataAnnotations;
using CIS.Foms.Enums;

namespace ExternalServices.ESignatures.Configuration;

public class ESignaturesConfiguration
{
    [Required]
    public string ConnectionString { get; set; } = null!;

    public ServiceImplementationTypes ImplementationType { get; set; } = ServiceImplementationTypes.Unknown;
}