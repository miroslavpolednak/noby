﻿namespace CIS.Core.Exceptions.ExternalServices;

/// <summary>
/// Služba třetí strany (ExternalServices) není dostupná - např. špatné URL volané služby, nebo volaná služba vůbec neběží.
/// </summary>
public sealed class CisExternalServiceUnavailableException
    : BaseCisException
{
    /// <summary>
    /// Název služby, která selhala
    /// </summary>
    public string ServiceName { get; init; }

    /// <summary>
    /// URI jehož volání selhalo
    /// </summary>
    public string? RequestUri { get; init; }

    /// <param name="serviceName">Název služby, která selhala</param>
    /// <param name="requestUri">URI jehož volání selhalo</param>
    /// <param name="message">Textový popis chyby</param>
    public CisExternalServiceUnavailableException(string serviceName, string requestUri, string message)
        : base(5, message)
    {
        RequestUri = requestUri;
        ServiceName = serviceName;
    }

    public CisExternalServiceUnavailableException(int exceptionCode, string serviceName)
        : base(exceptionCode, $"External service {serviceName} is unavailable")
    {
        ServiceName = serviceName;
    }
}