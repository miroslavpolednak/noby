using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
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
    private readonly string _appComponent;

    public KbHeadersHttpHandler(string appComponent = "NOBY")
    {
        _appComponent = appComponent;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("X-KB-Caller-System-Identity", $$"""{"app":"NOBY","appComp":"{{_appComponent}}"}""");
        if (Activity.Current?.Id is not null)
        {
            request.Headers.Add("X-B3-TraceId", Activity.Current?.RootId);
            request.Headers.Add("X-B3-SpanId", Activity.Current?.SpanId.ToString());
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
