namespace NOBY.Infrastructure.ErrorHandling;

public struct ApiErrorItem
{
    public ApiErrorItem(int code)
    {
        var item = ErrorCodeMapper.Messages[code];

        this.ErrorCode = code;
        this.Message = item.Message;
        this.Description = item.Description;
        this.Severity = item.Severity;
    }

    public ApiErrorItem(int code, string message, ApiErrorItemServerity severity)
    {
        this.ErrorCode = code;
        this.Message = message;
        this.Severity = severity;
    }

    public ApiErrorItem(int code, string message, string description, ApiErrorItemServerity severity)
    {
        this.ErrorCode = code;
        this.Message = message;
        this.Severity = severity;
        this.Description = description;
    }

    /// <summary>
    /// Kod chyby v rozsahu FE API
    /// </summary>
    public int ErrorCode { get; set; }

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

    /// <summary>
    /// Upresneni duvodu chyby - napr. nefunkcni intergrace na treti strany
    /// </summary>
    public ErrorReason? Reason { get; set; }

    public sealed class ErrorReason
    {
        /// <summary>
        /// Typ problemu.
        /// </summary>
        /// <remarks>
        /// ExternalService unavailable
        /// ExternalService server error
        /// </remarks>
        public string ReasonType { get; set; } = null!;

        /// <summary>
        /// Popis problemu, napr. nazev nefunkcni integrace na treti strany
        /// </summary>
        public string? ReasonDescription { get; set; }
    }
}
