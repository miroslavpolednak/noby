namespace DomainServices.CaseService.Api;

internal sealed class AppConfiguration
{
    /// <summary>
    /// Konfigurace EAS-SB sluzby
    /// </summary>
    public ExternalServices.Eas.EasConfiguration? EAS { get; set; }

    /// <summary>
    /// Konfigurace EAS-EasSimulationHT sluzby
    /// </summary>
    public ExternalServices.EasSimulationHT.EasSimulationHTConfiguration? EasSimulationHT { get; set; }
}
