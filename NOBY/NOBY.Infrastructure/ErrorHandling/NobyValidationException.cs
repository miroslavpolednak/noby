﻿namespace NOBY.Infrastructure.ErrorHandling;

public sealed class NobyValidationException
    : Exception
{
    public int HttpStatusCode { get; init; } = 400;

    /// <summary>
    /// Seznam chyb.
    /// </summary>
    public IReadOnlyList<ApiErrorItem> Errors { get; init; }

    public NobyValidationException(int exceptionCode, int httpStatusCode)
        : this(exceptionCode)
    {
        HttpStatusCode = httpStatusCode;
    }

    public NobyValidationException(string message, int httpStatusCode)
        : this(message)
    {
        HttpStatusCode = httpStatusCode;
    }

    public NobyValidationException(int exceptionCode)
    {
        if (!ErrorCodeMapper.Messages.ContainsKey(exceptionCode))
        {
            throw new CisNotFoundException(0, $"Error code item #{exceptionCode} not found in ErrorCodeMapper");
        }

        var item = ErrorCodeMapper.Messages[exceptionCode];
        this.Errors = new List<ApiErrorItem>
        {
            new() 
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
        : this(ErrorCodeMapper.DefaultExceptionCode, ErrorCodeMapper.Messages[ErrorCodeMapper.DefaultExceptionCode].Message, message)
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
            new(exceptionCode, message, ApiErrorItemServerity.Error)
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
            new(exceptionCode, message, description, ApiErrorItemServerity.Error)
        }.AsReadOnly();
    }

    public NobyValidationException(IEnumerable<ApiErrorItem> errors)
    {
        this.Errors = errors.ToList().AsReadOnly();
    }
}
