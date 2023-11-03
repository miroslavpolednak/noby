namespace CIS.Infrastructure.CisMediatR.PayloadLogger;

public sealed class PayloadLoggerBehaviorConfiguration
{
    /// <summary>
    /// True = do logu se ulozi plny request payload sluzby
    /// </summary>
    public bool LogRequestPayload { get; set; }

    /// <summary>
    /// True = do logu se ulozi plny response payload sluzby
    /// </summary>
    public bool LogResponsePayload { get; set; }
}
