namespace NOBY.Infrastructure.ErrorHandling;

public struct ApiErrorItem
{
    public ApiErrorItem(string code, string message, ApiErrorItemServerity severity)
    {
        this.ErrorCode = code;
        this.Message = message;
        this.Severity = severity;
    }

    public ApiErrorItem(string code, string message, string description, ApiErrorItemServerity severity)
    {
        this.ErrorCode = code;
        this.Message = message;
        this.Severity = severity;
        this.Description = description;
    }

    /// <summary>
    /// Kod chyby v rozsahu FE API
    /// </summary>
    public string ErrorCode { get; set; }

    /// <summary>
    /// Chybova zprava
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Popis chyby
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Zavaznost chyby
    /// </summary>
    public ApiErrorItemServerity Severity { get; set; }
}

/// <summary>
/// Zavaznost chyby
/// </summary>
public enum ApiErrorItemServerity
{
    Error = 1,
    Warning = 2
}
