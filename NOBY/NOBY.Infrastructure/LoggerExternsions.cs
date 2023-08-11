using Microsoft.Extensions.Logging;

namespace NOBY.Infrastructure;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, string, Exception> _userWithoutAccess;

    static LoggerExtensions()
    {
        _userWithoutAccess = LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.SecurityUserWithoutAccess, nameof(UserWithoutAccess)),
            "Cookie handler: user '{Login}' does not have APPLICATION_BasicAccess");
    }

    public static void UserWithoutAccess(this ILogger logger, string login)
        => _userWithoutAccess(logger, login, null!);
}