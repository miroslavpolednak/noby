namespace DomainServices.SalesArrangementService.Api;

internal sealed class AppConfiguration
{
    /// <summary>
    /// Konfigurace EAS-SB sluzby
    /// </summary>
    public ExternalServices.Eas.EasConfiguration? EAS { get; set; }

    /// <summary>
    /// Konfigurace Rip sluzby (Risk Integration Platform)
    /// </summary>
    public ExternalServices.Rip.RipConfiguration? Rip { get; set; }
}
