namespace CIS.Infrastructure.ExternalServicesHelpers;

public static class HttpHandlersExtensions
{
    /// <summary>
    /// Přidá do HttpClient try-catch tak, aby se nevraceli výchozí vyjímky, ale jejich CIS ekvivalenty.
    /// </summary>
    /// <param name="serviceName">Název ExternalServices proxy</param>
    public static IHttpClientBuilder AddExternalServicesErrorHandling(this IHttpClientBuilder builder, string serviceName)
        => builder.AddHttpMessageHandler(b => new HttpHandlers.ErrorHandlingHttpHandler(serviceName));

    /// <summary>
    /// Prida do kazdeho requestu HttpClienta hlavicky vyzadovane v KB.
    /// </summary>
    /// <param name="appComponent">Hodnota appComp v hlavičce X-KB-Caller-System-Identity. Pokud není vyplněno, je nastavena na "NOBY".</param>
    public static IHttpClientBuilder AddExternalServicesKbHeaders(this IHttpClientBuilder builder, string? appComponent = null, string? appComponentOriginator = null)
        => builder.AddHttpMessageHandler(b => new HttpHandlers.KbHeadersHttpHandler(appComponent, appComponentOriginator));

    /// <summary>
    /// Prida do kazdeho requestu HttpClienta hlavicku s aktualnim uzivatelem vyzadovanou v KB.
    /// </summary>
    public static IHttpClientBuilder AddExternalServicesKbPartyHeaders(this IHttpClientBuilder builder)
        => builder.AddHttpMessageHandler(b => new HttpHandlers.KbPartyHeaderHttpHandler(b));

    /// <summary>
    /// Doplňuje do každého requestu Correlation Id z OT.
    /// </summary>
    /// <param name="headerKey">Klíč v hlavičce, kam se má Id zapsat. Pokud není vyplněno, ne nastavena na "X-Correlation-ID".</param>
    public static IHttpClientBuilder AddExternalServicesCorrelationIdForwarding(this IHttpClientBuilder builder, string? headerKey = null)
        => builder.AddHttpMessageHandler(b => new HttpHandlers.CorrelationIdForwardingHttpHandler(headerKey));
}
