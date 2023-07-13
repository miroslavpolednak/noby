using CIS.Core.Security;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace CIS.Infrastructure.Telemetry.Enrichers;

/// <summary>
/// Doda do kontextu log item ID a Login uzivatele
/// </summary>
internal sealed class CisHeadersEnricher
    : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var ctx = _httpContextAccessor.HttpContext;
        if (ctx == null) return;

        var httpContextCache = ctx.Items[_httpCacheKey] as HttpHeadersCache;

        // prvni call v ramci tohoto http requestu
        if (httpContextCache is null)
        {
            // napln cache hodnotami z hlavicek/service
            httpContextCache = createHttpHeadersCache();
            ctx.Items[_httpCacheKey] = httpContextCache;
        }

        if (httpContextCache.CisUserId.HasValue)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(Constants.LoggerContextUserIdPropertyName, httpContextCache.CisUserId));
        }

        if (!string.IsNullOrEmpty(httpContextCache.CisUserIdent))
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(Constants.LoggerContextUserIdentPropertyName, httpContextCache.CisUserIdent));
        }
    }

    private HttpHeadersCache createHttpHeadersCache()
    {
        var userAccessor = _httpContextAccessor
            .HttpContext?
            .RequestServices?
            .GetService(typeof(ICurrentUserAccessor)) as ICurrentUserAccessor;
        
        var result = Helpers.GetCurrentUser(userAccessor, _httpContextAccessor);

        return new HttpHeadersCache
        {
            CisUserId = result.UserId,
            CisUserIdent = result.UserIdent
        };
    }

    private sealed class HttpHeadersCache
    {
        public int? CisUserId { get; set; }
        public string? CisUserIdent { get; set; }
    }

    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string _httpCacheKey = "serilog-enrichers-cis-headers";

    public CisHeadersEnricher(IHttpContextAccessor contextAccessor)
    {
        _httpContextAccessor = contextAccessor;
    }
}
