using System.Collections.Immutable;
using System.Globalization;

namespace CIS.Core.Exceptions;

/// <summary>
/// Validační chyba.
/// </summary>
/// <remarks>
/// Vyhazujeme v případě, že chceme propagovat ošetřené chyby v byznys logice - primárně z FluentValidation.
/// Může také ošetřovat stavy místo ArgumentException nebo ArgumentNullException a podobně.
/// </remarks>
public class CisValidationException
    : Exception
{
    /// <summary>
    /// Seznam chyb.
    /// </summary>
    public ImmutableList<CisExceptionItem> Errors { get; init; }

    /// <param name="message">Chybová zpráva</param>
    public CisValidationException(string message)
        : this(BaseCisException.UnknownExceptionCode, message)
    { }

    /// <param name="exceptionCode">CIS error kód</param>
    /// <param name="message">Chybová zpráva</param>
    public CisValidationException(int exceptionCode, string message)
        : this(exceptionCode.ToString(CultureInfo.InvariantCulture), message)
    { }

    /// <param name="exceptionCode">CIS error kód</param>
    /// <param name="message">Chybová zpráva</param>
    public CisValidationException(string exceptionCode, string message)
        : base(message)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(message);

        this.Errors = new List<CisExceptionItem>
        {
            new(exceptionCode, message)
        }.ToImmutableList();
    }

    /// <param name="errors">Seznam chyb</param>
    public CisValidationException(ImmutableList<CisExceptionItem> errors)
        : base(errors.FirstOrDefault()?.Message)
    {
        if (errors is null || !errors.Any())
            throw new ArgumentNullException(nameof(errors), $"No errors has been specified when creating new CisValidationException");

        this.Errors = errors;
    }

    /// <param name="errors">Seznam chyb</param>
    public CisValidationException(IEnumerable<CisExceptionItem> errors)
        : base(errors.FirstOrDefault()?.Message)
    {
        if (errors is null || !errors.Any())
            throw new ArgumentNullException(nameof(errors), $"No errors has been specified when creating new CisValidationException");

        this.Errors = errors.ToImmutableList();
    }
}