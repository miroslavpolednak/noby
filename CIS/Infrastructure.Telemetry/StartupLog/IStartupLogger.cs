namespace CIS.Infrastructure.Telemetry;

public interface IStartupLogger
{
    void RegisteringServices();

    void ApplicationBuilt();

    void ApplicationRun();

    void ApplicationFinished();

    void CatchedException(Exception exception);

    void CloseAndFlush();
}
