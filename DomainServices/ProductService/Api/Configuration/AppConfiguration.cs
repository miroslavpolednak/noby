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
    public ExternalServices.MpHome.MpHomeConfiguration? MpHome { get; set; }
}
