namespace NOBY.Infrastructure.ErrorHandling;

public struct ApiErrorItem
{
    public int ErrorCode { get; set; }

    public string Message { get; set; }

    public ApiErrorItemServerity Severity { get; set; }
}

public enum ApiErrorItemServerity
{
    Error = 1,
    Warning = 2
}
