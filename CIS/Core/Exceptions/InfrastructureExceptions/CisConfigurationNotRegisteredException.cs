namespace CIS.Core.Exceptions;

/// <summary>
/// Chyba v pripade, kdy nelze v DI najit instanci konfigurace CIS
/// </summary>
public sealed class CisConfigurationNotRegisteredException : BaseCisException
{
    public CisConfigurationNotRegisteredException(string parameter)
        : base(1, $"No instance of ICisEnvironmentConfiguration registered or parameter '{parameter}' is null. Did you run services.AddCisEnvironmentConfiguration() in prior to add caching features?") { }

    public CisConfigurationNotRegisteredException()
        : base(2, "No instance of ICisEnvironmentConfiguration registered. Did you run services.AddCisEnvironmentConfiguration() in prior to add caching features?") { }
}