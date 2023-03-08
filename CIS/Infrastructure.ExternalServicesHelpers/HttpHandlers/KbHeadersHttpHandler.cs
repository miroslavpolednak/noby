using System.Diagnostics;

namespace CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers;

/// <summary>
/// Middleware přidávájící KB hlavičky do requestu.
/// </summary>
/// <remarks>
/// Přidává hlavičky X-KB-Caller-System-Identity a X-B3-TraceId a X-B3-SpanId.
/// </remarks>
public sealed class KbHeadersHttpHandler
    : DelegatingHandler
{
    public const string DefaultAppValue = "NOBY";
    public const string DefaultAppCompOriginatorValue = "NOBY.FEAPI";
    public const string DefaultAppCompValue = "NOBY.DS";

    private readonly string _appComponent;
    private readonly string _appComponentOriginator;

    public KbHeadersHttpHandler(string? appComponent = null, string? appComponentOriginator = null)
    {
        _appComponent = appComponent ?? DefaultAppCompValue;
        _appComponentOriginator = appComponentOriginator ?? DefaultAppCompOriginatorValue;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("X-KB-Orig-System-Identity", $$"""{"app":"{{DefaultAppValue}}","appComp":"{{_appComponentOriginator}}"}""");
        request.Headers.Add("X-KB-Caller-System-Identity", $$"""{"app":"{{DefaultAppValue}}","appComp":"{{_appComponent}}"}""");

        // trace info
        if (Activity.Current?.Id is not null)
        {
            request.Headers.Add("X-B3-TraceId", Activity.Current?.RootId);
            request.Headers.Add("X-B3-SpanId", Activity.Current?.SpanId.ToString());
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
