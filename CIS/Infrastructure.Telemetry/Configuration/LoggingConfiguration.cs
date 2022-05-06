﻿namespace CIS.Infrastructure.Telemetry.Configuration;

internal sealed class LoggingConfiguration
{
    public LogBehaviourTypes LogType { get; set; }

    public LogConfiguration? Application { get; set; }

    public LogConfiguration? Audit { get; set; }
}
