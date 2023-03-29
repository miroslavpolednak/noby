namespace CIS.Core.Exceptions.ExternalServices;

/// <summary>
/// HTTP 400. Chyba, která vzniká při volání API třetích stran. Pokud API vrátí HTTP 4xx, vytáhneme z odpovědi chybu a vyvoláme tuto vyjímku. Podporuje seznam chyb.
/// </summary>
public sealed class CisExtServiceValidationException
    : CisValidationException
{
    /// <param name="message">Chybová zpráva</param>
    public CisExtServiceValidationException(string message)
        : base(BaseCisException.UnknownExceptionCode, message)
    { }

    /// <param name="exceptionCode">CIS error kód</param>
    /// <param name="message">Chybová zpráva</param>
    public CisExtServiceValidationException(int exceptionCode, string message)
        : base(exceptionCode, message)
    { }

    /// <param name="exceptionCode">CIS error kód</param>
    /// <param name="message">Chybová zpráva</param>
    public CisExtServiceValidationException(string exceptionCode, string message)
        : base(exceptionCode, message)
    { }

    /// <param name="errors">Seznam chyb</param>
    public CisExtServiceValidationException(IEnumerable<CisExceptionItem> errors)
        : base(errors)
    { }
}