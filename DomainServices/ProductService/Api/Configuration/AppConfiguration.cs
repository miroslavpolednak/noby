namespace DomainServices.ProductService.Api;

internal sealed class AppConfiguration
{
    /// <summary>
    /// Konfigurace EAS-SB sluzby
    /// </summary>
    public ExternalServices.Eas.EasConfiguration? EAS { get; set; }

    /// <summary>
    /// Konfigurace MP Home sluzby
    /// </summary>
    public ExternalServiceConfiguration? MpHome { get; set; }

    public class ExternalServiceConfiguration
    {
        /// <summary>
        /// Druh implementace sluzby
        /// </summary>
        public CIS.Core.ServiceImplementationTypes Implementation { get; set; }

        /// <summary>
        /// URL endpointu
        /// </summary>
        public string? ServiceUrl { get; set; }
    }
}
