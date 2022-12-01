namespace CIS.Core.Exceptions.ExternalServices;

public sealed class CisExtServiceServerErrorException
    : BaseCisException
{
    /// <summary>
    /// Název služby, která selhala
    /// </summary>
    public string ServiceName { get; init; }

    /// <summary>
    /// URI jehož volání selhalo
    /// </summary>
    public string RequestUri { get; init; }

    /// <param name="serviceName">Název služby, která selhala</param>
    /// <param name="requestUri">URI jehož volání selhalo</param>
    /// <param name="message">Textový popis chyby</param>
    public CisExtServiceServerErrorException(string serviceName, string requestUri, string message)
        : base(15, message)
    {
        RequestUri = requestUri;
        ServiceName = serviceName;
    }
}
