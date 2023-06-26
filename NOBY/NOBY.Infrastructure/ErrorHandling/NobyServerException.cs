namespace NOBY.Infrastructure.ErrorHandling;

public sealed class NobyServerException
    : Exception
{
    public ApiErrorItem Error { get; init; }

    public NobyServerException(int exceptionCode)
    {
        if (!ErrorCodeMapper.Messages.ContainsKey(exceptionCode))
        {
            throw new CisNotFoundException(0, $"Error code item #{exceptionCode} not found in ErrorCodeMapper");
        }

        var item = ErrorCodeMapper.Messages[exceptionCode];
        this.Error = new ApiErrorItem
        {
            ErrorCode = exceptionCode,
            Message = item.Message,
            Description = item.Description,
            Severity = item.Severity
        };
    }

    /// <param name="exceptionCode">CIS error kód</param>
    /// <param name="message">Chybová zpráva</param>
    public NobyServerException(int exceptionCode, string message)
        : base(message)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(message);

        if (!ErrorCodeMapper.Messages.ContainsKey(exceptionCode))
        {
            throw new ArgumentException($"Error code #{exceptionCode} is already in use in ErrorCodeMapper", nameof(exceptionCode));
        }

        this.Error = new ApiErrorItem(exceptionCode, message, ApiErrorItemServerity.Error);
    }
}
