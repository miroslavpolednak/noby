namespace DomainServices.OfferService.Api;

internal sealed class AppConfiguration
{
    public int BuldingSavingsProductInstanceType { get; set; }

    /// <summary>
    /// Konfigurace EAS-SB sluzby
    /// </summary>
    public ExternalServices.Eas.EasConfiguration? EAS { get; set; }
}
