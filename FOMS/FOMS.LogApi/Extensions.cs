namespace FOMS.LogApi;

internal static class Extensions
{
    public static LogLevel ToLogLevel(this int level)
        => level switch
        {
            1 => LogLevel.Trace,
            2 => LogLevel.Debug,
            3 => LogLevel.Information,
            4 => LogLevel.Warning,
            5 => LogLevel.Error,
            _ => LogLevel.None
        };
}
