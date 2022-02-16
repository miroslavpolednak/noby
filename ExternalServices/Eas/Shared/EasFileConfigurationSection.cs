namespace ExternalServices.Eas;

public sealed class EasConfigurationFileSection
{
    /// <summary>
    /// Verze implementace EAS
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
