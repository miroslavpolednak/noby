using CIS.Core.Exceptions;

namespace NOBY.Infrastructure.ErrorHandling;

public sealed class NobyValidationException
    : Exception
{
    public const int DefaultExceptionCode = 90001;

    /// <summary>
    /// Seznam chyb.
    /// </summary>
    public IReadOnlyList<ApiErrorItem> Errors { get; init; }

    public NobyValidationException(int exceptionCode)
    {
        if (!ErrorCodeMapper.Messages.ContainsKey(exceptionCode))
        {
            throw new CisNotFoundException(0, $"Error code item #{exceptionCode} not found in ErrorCodeMapper");
        }

        var item = ErrorCodeMapper.Messages[exceptionCode];
        this.Errors = new List<ApiErrorItem>
        {
            new ApiErrorItem
            {
                ErrorCode = exceptionCode,
                Message = item.Message,
                Description = item.Description,
                Severity = item.Severity
            }
        }.AsReadOnly();
    }

    /// <param name="message">Chybová zpráva</param>
    public NobyValidationException(string message)
        : this(DefaultExceptionCode, ErrorCodeMapper.Messages[DefaultExceptionCode].Message, message)
    { }

    /// <param name="exceptionCode">CIS error kód</param>
    /// <param name="message">Chybová zpráva</param>
    public NobyValidationException(int exceptionCode, string message)
        : base(message)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(message);

        if (!ErrorCodeMapper.Messages.ContainsKey(exceptionCode))
        {
            throw new ArgumentException($"Error code #{exceptionCode} is already in use in ErrorCodeMapper", nameof(exceptionCode));
        }

        this.Errors = new List<ApiErrorItem>
        {
            new ApiErrorItem(exceptionCode, message, ApiErrorItemServerity.Error)
        }.AsReadOnly();
    }

    public NobyValidationException(int exceptionCode, string message, string description)
        : base(message)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(message);
        ArgumentNullException.ThrowIfNullOrEmpty(description);

        if (!ErrorCodeMapper.Messages.ContainsKey(exceptionCode))
        {
            throw new ArgumentException($"Error code #{exceptionCode} is already in use in ErrorCodeMapper", nameof(exceptionCode));
        }

        this.Errors = new List<ApiErrorItem>
        {
            new ApiErrorItem(exceptionCode, message, description, ApiErrorItemServerity.Error)
        }.AsReadOnly();
    }

    public NobyValidationException(IEnumerable<ApiErrorItem> errors)
    {
        this.Errors = errors.ToList().AsReadOnly();
    }
}
