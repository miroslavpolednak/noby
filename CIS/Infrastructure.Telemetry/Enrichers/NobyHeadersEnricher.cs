using CIS.Core.Security;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace CIS.Infrastructure.Telemetry.Enrichers;

/// <summary>
/// Doda do kontextu log item ID a Login uzivatele
/// </summary>
internal class NobyHeadersEnricher
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

        if (httpContextCache.NobyUserId.HasValue)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(Constants.LoggerContextUserIdPropertyName, httpContextCache.NobyUserId));
        }

        if (!string.IsNullOrEmpty(httpContextCache.NobyUserIdent))
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(Constants.LoggerContextUserIdentPropertyName, httpContextCache.NobyUserIdent));
        }
    }

    private HttpHeadersCache createHttpHeadersCache()
    {
        var headersCache = new HttpHeadersCache();

        var userAccessor = _httpContextAccessor
            .HttpContext?
            .RequestServices?
            .GetService(typeof(ICurrentUserAccessor)) as ICurrentUserAccessor;

        // mam v kontextu instanci uzivatele
        if (userAccessor is not null && userAccessor.IsAuthenticated)
        {
            headersCache.NobyUserId = userAccessor.User!.Id;
            headersCache.NobyUserIdent = userAccessor.User!.Login;
        }
        // neni instance uzivatele, zkus se kouknout do http hlavicek
        else if (hasKey(SecurityConstants.ContextUserHttpHeaderUserIdKey))
        {
            if (int.TryParse(_httpContextAccessor.HttpContext!.Request.Headers[SecurityConstants.ContextUserHttpHeaderUserIdKey].First(), out int userId))
            {
                headersCache.NobyUserId = userId;
            }
            
            if (hasKey(SecurityConstants.ContextUserHttpHeaderUserIdentKey))
            {
                headersCache.NobyUserIdent = _httpContextAccessor.HttpContext!.Request.Headers[SecurityConstants.ContextUserHttpHeaderUserIdentKey].First();
            }
        }
        // posledni pokus - muze byt jiz vytvorena claims identity, ale jeste neni v kontextu User z auth middlewaru
        else if (_httpContextAccessor.HttpContext?.User?.HasClaim(t => t.Type == SecurityConstants.ClaimTypeIdent) ?? false)
        {
            headersCache.NobyUserIdent = _httpContextAccessor.HttpContext!.User.Claims.First(t => t.Type == SecurityConstants.ContextUserHttpHeaderUserIdentKey).Value;
        }

        return headersCache;
    }

    private bool hasKey(string key)
    {
        return _httpContextAccessor.HttpContext?.Request?.Headers?.ContainsKey(key) ?? false;
    }

    private sealed class HttpHeadersCache
    {
        public int? NobyUserId { get; set; }
        public string? NobyUserIdent { get; set; }
    }

    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string _httpCacheKey = "serilog-enrichers-noby-headers";

    public NobyHeadersEnricher(IHttpContextAccessor contextAccessor)
    {
        _httpContextAccessor = contextAccessor;
    }
}
