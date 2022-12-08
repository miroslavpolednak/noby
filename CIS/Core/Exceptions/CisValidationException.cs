using System.Collections.Immutable;
using System.Runtime.Serialization;

namespace CIS.Core.Exceptions;

/// <summary>
/// HTTP 400. Validační chyba.
/// </summary>
/// <remarks>
/// Vyhazujeme v případě, že chceme propagovat ošetřené chyby v byznys logice - primárně z FluentValidation. Podporuje seznam chyb.
/// </remarks>
public sealed class CisValidationException 
    : BaseCisValidationException
{
    /// <param name="message">Chybová zpráva</param>
    public CisValidationException(string message)
        : base(BaseCisException.UnknownExceptionCode, message)
    { }

    /// <param name="exceptionCode">CIS error kód</param>
    /// <param name="message">Chybová zpráva</param>
    public CisValidationException(int exceptionCode, string message) 
        : base(exceptionCode, message)
    { }

    /// <param name="errors">Seznam chyb</param>
    /// <param name="message">Souhrná chybová zpráva</param>
    public CisValidationException(IEnumerable<(string Key, string Message)> errors, string message = "") 
        : base(errors, message)
    { }

    /// <param name="errors">Seznam chyb</param>
    /// <param name="message">Souhrná chybová zpráva</param>
    public CisValidationException(IEnumerable<(int Key, string Message)> errors, string message = "")
        : base(errors, message)
    {  }
}
