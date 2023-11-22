namespace CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers;

/// <summary>
/// Middleware pro logování payloadu a hlavičke requestu a responsu.
/// </summary>
/// <remarks>
/// Vloží do kontextu logovaného záznamu klíče Payload a Headers s odpovídajícími objekty. Pokud např. response payload neobsahuje, není tento klíč do kontextu logovaného záznamu vložen.
/// </remarks>
public sealed class LoggingHttpHandler 
    : DelegatingHandler
{
    private readonly bool _logRequestPayload;
    private readonly bool _logResponsePayload;
    private readonly ILogger _logger;

    private const int _maxPayloadTrashold = 1024 * 512;

    public LoggingHttpHandler(HttpMessageHandler innerHandler, ILogger logger, bool logRequestPayload = true, bool logResponsePayload = true)
        : base(innerHandler)
    {
        _logRequestPayload = logRequestPayload;
        _logResponsePayload = logResponsePayload;
        _logger = logger;
    }

    public LoggingHttpHandler(ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory);

        _logger = loggerFactory.CreateLogger<LoggingHttpHandler>();
    }

#pragma warning disable CS8604 // Possible null reference argument.
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Content is not null && _logRequestPayload)
        {
            var payloadData = new Dictionary<string, object>
            {
                { "Headers", request.Headers?.ToDictionary(x => x.Key, v => string.Join(';', v.Value)) }
            };
            // streamy nelogujeme
            if (request.Content is ByteArrayContent || request.Content is StreamContent)
            {
                payloadData.Add("Payload", "[binary data]");
            }
            else
            {
                var payload = await request.Content!.ReadAsStringAsync(cancellationToken);
                CIS.Core.StringExtensions.TrimUtf8String(ref payload, _maxPayloadTrashold);
                payloadData.Add("Payload", payload);
            }

            using (_logger.BeginScope(payloadData))
            {
                _logger.HttpRequestPayload(request);
            }
        }
        else if (request.Headers is not null)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                { "Headers", request.Headers.ToDictionary(x => x.Key, v => string.Join(';', v.Value)) }
            }))
            {
                _logger.HttpRequestStarted(request);
            }
        }

        var response = await base.SendAsync(request, cancellationToken);

        int statusCode = (int)response.StatusCode;

        // logovat vsechen response
        if (response?.Content is not null && _logResponsePayload)
        {
            var payload = response.Content == null ? string.Empty : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            CIS.Core.StringExtensions.TrimUtf8String(ref payload, _maxPayloadTrashold);
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                { "Payload", payload },
                { "Headers", response.Headers?.ToDictionary(x => x.Key, v => string.Join(';', v.Value)) }
            }))
            {
                _logger.HttpResponsePayload(request, statusCode);
            }
        }
        else
        {
            _logger.HttpResponseFinished(request, statusCode);
        }

        return response!;     
    }
}

