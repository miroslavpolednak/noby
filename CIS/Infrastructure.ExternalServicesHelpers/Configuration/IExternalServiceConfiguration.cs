﻿namespace CIS.Infrastructure.ExternalServicesHelpers.Configuration;

/// <summary>
/// Základní konfigurace externí služby (služby třetí strany).
/// </summary>
/// <remarks>Pro registraci HTTP klienta by se vždy měla používat generická verze interface.</remarks>
public interface IExternalServiceConfiguration
    : CIS.Core.IIsServiceDiscoverable
{
    /// <summary>
    /// Pokud je true, pouzije pro HttpClient systemovou proxy
    /// </summary>
    bool UseDefaultProxy { get; set; }

    /// <summary>
    /// Zapne logovani request a response payloadu a hlavicek. Default: true
    /// </summary>
    /// <remarks>Je v konfiguraci, aby bylo možné měnit nastavení na úrovni CI/CD.</remarks>
    bool UseLogging { get; set; }

    /// <summary>
    /// True = do logu se ulozi plny payload odpovedi externi sluzby
    /// </summary>
    bool LogRequestPayload { get; set; }

    /// <summary>
    /// True = do logu se ulozi plny request poslany do externi sluzby
    /// </summary>
    bool LogResponsePayload { get; set; }

    /// <summary>
    /// Default request timeout in seconds
    /// </summary>
    /// <remarks>Default is set to 5 seconds</remarks>
    int? RequestTimeout { get; set; }

    /// <summary>
    /// Apply retry policy on HttpRequest with supplied retry count
    /// </summary>
    /// <remarks>Default is set to 3</remarks>
    int? RequestRetryCount { get; set; }

    /// <summary>
    /// Time between consequent retry requests in seconds
    /// </summary>
    /// <remarks>Default is set to 5s</remarks>
    int? RequestRetryTimeout { get; set; }

    /// <summary>
    /// Pokud =true, ignoruje HttpClient problem s SSL certifikatem remote serveru.
    /// </summary>
    bool IgnoreServerCertificateErrors { get; set; }

    /// <summary>
    /// Type of http client implementation - can be mock or real client or something else.
    /// </summary>
    ServiceImplementationTypes ImplementationType { get; set; }

    /// <summary>
    /// Typ pouzite autentizace na sluzbu treti strany
    /// </summary>
    ExternalServicesAuthenticationTypes Authentication { get; set; }

    /// <summary>
    /// Autentizace - Username
    /// </summary>
    string? Username { get; set; }

    /// <summary>
    /// Autentizace - Heslo
    /// </summary>
    string? Password { get; set; }
}

/// <summary>
/// Generická verze konfigurace.
/// </summary>
/// <typeparam name="TClient">Typ HTTP klienta</typeparam>
public interface IExternalServiceConfiguration<TClient>
    : IExternalServiceConfiguration
    where TClient : class, IExternalServiceClient
{ }
