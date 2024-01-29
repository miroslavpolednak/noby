namespace CIS.Core.Exceptions.ExternalServices;

/// <summary>
/// HTTP 400. Chyba, která vzniká při volání API třetích stran. Pokud API vrátí HTTP 4xx, vytáhneme z odpovědi chybu a vyvoláme tuto vyjímku. Podporuje seznam chyb.
/// </summary>
public sealed class CisExternalServiceValidationException
    : CisValidationException
{
    /// <param name="message">Chybová zpráva</param>
    public CisExternalServiceValidationException(string message)
        : base(BaseCisException.UnknownExceptionCode, message)
    { }

    /// <param name="exceptionCode">CIS error kód</param>
    /// <param name="message">Chybová zpráva</param>
    public CisExternalServiceValidationException(int exceptionCode, string message)
        : base(exceptionCode, message)
    { }

    /// <param name="exceptionCode">CIS error kód</param>
    /// <param name="message">Chybová zpráva</param>
    public CisExternalServiceValidationException(string exceptionCode, string message)
        : base(exceptionCode, message)
    { }

    /// <param name="errors">Seznam chyb</param>
    public CisExternalServiceValidationException(IEnumerable<CisExceptionItem> errors)
        : base(errors)
    { }
}