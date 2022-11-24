namespace CIS.Core.Exceptions;

/// <summary>
/// Pokud chybí požadované nastavení konfigurace v appsettings.json
/// </summary>
public sealed class CisConfigurationNotFound 
    : BaseCisException
{
    /// <param name="sectionName">Název sekce v appsettings.json, která chybí</param>
    public CisConfigurationNotFound(string sectionName)
        : base(14, $"Configuration section '{sectionName}' not found")
    { }
}
