namespace ExternalServices.EasSimulationHT;

public sealed class EasSimulationHTFileConfigurationSection
{
    /// <summary>
    /// Verze implementace EasSimulationHT
    /// </summary>
    public Versions Version { get; set; }

    /// <summary>
    /// Druh implementace sluzby
    /// </summary>
    public CIS.Foms.Enums.ServiceImplementationTypes Implementation { get; set; }

    /// <summary>
    /// URL endpointu
    /// </summary>
    public string? ServiceUrl { get; set; }
}
