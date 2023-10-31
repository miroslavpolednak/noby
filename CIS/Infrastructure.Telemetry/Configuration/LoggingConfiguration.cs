using CIS.Core.Configuration;
using CIS.Core.Configuration.Telemetry;

namespace CIS.Infrastructure.Telemetry.Configuration;

public sealed class LoggingConfiguration
    : ILoggingConfiguration
{
    /// <summary>
    /// Typ logu - gRPC nebo WebApi
    /// </summary>
    public LogBehaviourTypes LogType { get; set; }

    /// <summary>
    /// Jak se má logovat - nastavení sinků
    /// </summary>
    public ILogConfiguration? Application { get; set; }

    /// <summary>
    /// Pokud je nastaveno, omezuje logování pouze na zadané RequestUrl
    /// </summary>
    public List<string>? IncludeOnlyPaths { get; set; }
}
