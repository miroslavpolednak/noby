namespace CIS.Core.Exceptions;

/// <summary>
/// Chyba validace názvu aplikace - vyvoláno z konstruktoru value type ApplicationKey
/// </summary>
public sealed class CisInvalidApplicationKeyException 
    : BaseCisException
{
    /// <param name="key">Název aplikace</param>
    public CisInvalidApplicationKeyException(string key) 
        : base(3, $"Application key '{key}' is invalid")
    { }
}
