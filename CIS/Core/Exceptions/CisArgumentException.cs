namespace CIS.Core.Exceptions;

/// <summary>
/// Stejná chyba jako <see cref="System.ArgumentException"/>, ale obsahuje navíc CIS error kód
/// </summary>
[Serializable]
public sealed class CisArgumentException
    : BaseCisArgumentException
{
    /// <param name="exceptionCode">CIS error kód</param>
    /// <param name="message">Chybová zpráva</param>
    /// <param name="paramName">Název parametru, který chybu vyvolal</param>
    public CisArgumentException(int exceptionCode, string message, string paramName)
        : base(exceptionCode, message, paramName)
    { }
}
