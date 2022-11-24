namespace CIS.Core.Exceptions;

/// <summary>
/// Chyba validace názvu prostředí - vyvoláno z konstruktoru value type EnvironmentName
/// </summary>
public sealed class CisInvalidEnvironmentNameException 
    : BaseCisArgumentException
{
    private const int _exceptionCode = 4;

    /// <param name="name">Název prostředí</param>
    public CisInvalidEnvironmentNameException(string name)
        : base(_exceptionCode, $"Environment Name '{name}' is invalid", "key")
    { }

    /// <param name="name">Název prostředí</param>
    /// <param name="paramName">Název parametru ve kterém bylo předáno prostředí</param>
    public CisInvalidEnvironmentNameException(string name, string paramName)
        : base(_exceptionCode, $"Environment Name '{name}' is invalid", paramName)
    { }
}
