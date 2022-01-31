namespace CIS.Core.Exceptions;

public sealed class CisConfigurationNotFound : BaseCisException
{
    public CisConfigurationNotFound(string sectionName)
        : base(14, $"Configuration section '{sectionName}' not found")
    { }
}
