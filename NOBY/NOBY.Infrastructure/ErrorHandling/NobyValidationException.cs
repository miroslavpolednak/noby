using System.Collections.Immutable;

namespace NOBY.Infrastructure.ErrorHandling;

public sealed class NobyValidationException
    : Exception
{
    public const string DefaultExceptionCode = "90001";

    /// <summary>
    /// Seznam chyb.
    /// </summary>
    public ImmutableList<ApiErrorItem> Errors { get; init; }

    /// <param name="message">Chybová zpráva</param>
    public NobyValidationException(string message)
        : this(DefaultExceptionCode, message)
    { }

    /// <param name="exceptionCode">CIS error kód</param>
    /// <param name="message">Chybová zpráva</param>
    public NobyValidationException(string exceptionCode, string message)
        : base(message)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(message);

        this.Errors = new List<ApiErrorItem>
        {
            new ApiErrorItem(exceptionCode, message, ApiErrorItemServerity.Error)
        }.ToImmutableList();
    }

    public NobyValidationException(string exceptionCode, string message, string description)
        : base(message)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(message);
        ArgumentNullException.ThrowIfNullOrEmpty(description);

        this.Errors = new List<ApiErrorItem>
        {
            new ApiErrorItem(exceptionCode, message, description, ApiErrorItemServerity.Error)
        }.ToImmutableList();
    }

    public NobyValidationException(IEnumerable<ApiErrorItem> errors)
    {
        this.Errors = errors.ToImmutableList();
    }
}
