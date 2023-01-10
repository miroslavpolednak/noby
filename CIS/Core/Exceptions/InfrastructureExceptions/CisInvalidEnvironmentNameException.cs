namespace CIS.Core.Exceptions;

/// <summary>
/// Chyba validace názvu prostředí - vyvoláno z konstruktoru value type EnvironmentName
/// </summary>
public sealed class CisInvalidEnvironmentNameException 
    : BaseCisException
{
    /// <param name="name">Název prostředí</param>
    public CisInvalidEnvironmentNameException(string name)
        : base(4, $"Environment Name '{name}' is invalid")
    { }

    /// <param name="name">Název prostředí</param>
    /// <param name="paramName">Název parametru ve kterém bylo předáno prostředí</param>
    public CisInvalidEnvironmentNameException(string name, string paramName)
        : base(4, $"Environment Name '{name}' is invalid")
    { }
}
