namespace CIS.Infrastructure.Telemetry.Configuration;

internal sealed class LoggingConfiguration
{
    /// <summary>
    /// Typ logu - gRPC nebo WebApi
    /// </summary>
    public LogBehaviourTypes LogType { get; set; }

    /// <summary>
    /// Jak se má logovat - nastavení sinků
    /// </summary>
    public LogConfiguration? Application { get; set; }

    /// <summary>
    /// Pokud je nastaveno, omezuje logování pouze na zadané RequestUrl
    /// </summary>
    public List<string>? IncludeOnlyPaths { get; set; }
}
