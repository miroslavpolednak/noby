using System.Collections.Immutable;

namespace CIS.Core.Exceptions.ExternalServices;

/// <summary>
/// HTTP 400. Chyba, která vzniká při volání API třetích stran. Pokud API vrátí HTTP 4xx, vytáhneme z odpovědi chybu a vyvoláme tuto vyjímku. Podporuje seznam chyb.
/// </summary>
public sealed class CisExtServiceValidationException
    : BaseCisValidationException
{
    /// <param name="message">Chybová zpráva</param>
    public CisExtServiceValidationException(string message)
        : base(UnknownExceptionCode, message)
    {
    }

    /// <param name="errors">Seznam chyb</param>
    /// <param name="message">Souhrná chybová zpráva</param>
    public CisExtServiceValidationException(IEnumerable<(string Key, string Message)> errors, string message = "")
        : base(UnknownExceptionCode, message)
    {
        Errors = errors.ToImmutableList();
    }
}
