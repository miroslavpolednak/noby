namespace CIS.Core.Exceptions;

/// <summary>
/// Stejná chyba jako <see cref="System.Exception"/>, ale obsahuje navíc CIS error kód
/// </summary>
public sealed class CisException
    : BaseCisException
{
    public CisException(int exceptionCode, string message)
        : base(exceptionCode, message)
    { }
}
