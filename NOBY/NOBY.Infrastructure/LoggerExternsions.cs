using Microsoft.Extensions.Logging;

namespace NOBY.Infrastructure;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, string, Exception> _userWithoutAccess;
    private static readonly Action<ILogger, string, Exception> _openIdError;

    static LoggerExtensions()
    {
        _userWithoutAccess = LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.SecurityUserWithoutAccess, nameof(UserWithoutAccess)),
            "Cookie handler: user '{Login}' does not have APPLICATION_BasicAccess");

        _openIdError = LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.SecurityOpenIdError, nameof(OpenIdError)),
            "OpenID error: {Operation}");
    }

    public static void UserWithoutAccess(this ILogger logger, string login)
        => _userWithoutAccess(logger, login, null!);

    public static void OpenIdError(this ILogger logger, string login)
        => _openIdError(logger, login, null!);
}