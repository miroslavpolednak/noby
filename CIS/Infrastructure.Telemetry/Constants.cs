namespace CIS.Infrastructure.Telemetry;

internal sealed class Constants
{
    public const string LoggerContextUserIdPropertyName = "CisUserId";

    public const string LoggerContextUserIdentPropertyName = "CisUserIdent";

    public const string FileLoggerTemplate = @"{Timestamp:yyyy-MM-dd HH:mm:ss,fff zzz} [{ThreadId}] [{Level:u}] [{TraceId}] [{SpanId}] [{ParentId}] [{CisAppKey}] [{Version}] [{Assembly}] [{SourceContext}] [{MachineName}] [{ClientIp}] [{CisUserId}] [{CisUserIdent}] [{RequestId}] [{RequestPath}] [{ConnectionId}] [{Message}] [{Exception}{Payload}]{NewLine}";
}
