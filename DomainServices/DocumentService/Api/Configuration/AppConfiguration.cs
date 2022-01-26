
using ExternalServices.ESignatures;

namespace DomainServices.DocumentService.Api;

internal sealed class AppConfiguration
{
    /// <summary>
    /// Konfigurace sluzby ePodpisy
    /// </summary>
    public ESignaturesConfiguration ESignatures { get; set; } = new ESignaturesConfiguration();
}
