namespace CIS.Core.Exceptions;

/// <summary>
/// Chyba validace názvu aplikace - vyvoláno z konstruktoru value type ApplicationKey
/// </summary>
public sealed class CisInvalidApplicationKeyException 
    : BaseCisArgumentException
{
    public new const int ExceptionCode = 3;

    /// <param name="key">Název aplikace</param>
    public CisInvalidApplicationKeyException(string key) 
        : base(ExceptionCode, $"Application key '{key}' is invalid", nameof(key))
    { }

    /// <param name="key">Název aplikace</param>
    public CisInvalidApplicationKeyException(string key, string paramName)
        : base(ExceptionCode, $"Application key '{key}' is invalid", paramName)
    { }
}
