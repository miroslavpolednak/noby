namespace CIS.Core.Configuration.Telemetry;

public interface ILoggingConfiguration
{
    /// <summary>
    /// Typ logu - gRPC nebo WebApi
    /// </summary>
    LogBehaviourTypes LogType { get; }

    /// <summary>
    /// Jak se má logovat - nastavení sinků
    /// </summary>
    ILogConfiguration? Application { get; }

    /// <summary>
    /// Pokud je nastaveno, omezuje logování pouze na zadané RequestUrl
    /// </summary>
    public List<string>? IncludeOnlyPaths { get; }
}
