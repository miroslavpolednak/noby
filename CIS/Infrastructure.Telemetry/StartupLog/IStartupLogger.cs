namespace CIS.Infrastructure.Telemetry;

public interface IStartupLogger
{
    void ApplicationBuilt();

    void ApplicationRun();

    void CatchedException(Exception exception);
}
