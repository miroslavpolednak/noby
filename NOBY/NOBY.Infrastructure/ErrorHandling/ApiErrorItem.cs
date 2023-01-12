namespace NOBY.Infrastructure.ErrorHandling;

public struct ApiErrorItem
{
    /// <summary>
    /// Kod chyby v rozsahu FE API
    /// </summary>
    public string ErrorCode { get; set; }

    /// <summary>
    /// Chybova zprava
    /// </summary>
    public string Message { get; set; }

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
