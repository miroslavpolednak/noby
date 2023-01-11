namespace CIS.Infrastructure.WebApi.Types;

public struct ApiErrorItem
{
    public string ErrorCode { get; set; }

    public string Message { get; set; }

    public ApiErrorItemServerity Severity { get; set; }
}

public enum ApiErrorItemServerity
{
    Error = 1,
    Warning = 2
}
