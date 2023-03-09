using CIS.Core.Security;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace CIS.Infrastructure.Telemetry.Enrichers;

internal class NobyHeadersEnricher
    : ILogEventEnricher
{
    private readonly IHttpContextAccessor _contextAccessor;

    public NobyHeadersEnricher(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var userAccessor = _contextAccessor
            .HttpContext?
            .RequestServices?
            .GetService(typeof(ICurrentUserAccessor)) as ICurrentUserAccessor;

        // mam v kontextu instanci uzivatele
        if (userAccessor is not null && userAccessor.IsAuthenticated)
        {
            var userIdentProperty = propertyFactory.CreateProperty(Constants.LoggerContextUserIdentPropertyName, userAccessor.User!.Login);
            logEvent.AddPropertyIfAbsent(userIdentProperty);

            var userIdProperty = propertyFactory.CreateProperty(Constants.LoggerContextUserIdPropertyName, userAccessor.User!.Id);
            logEvent.AddPropertyIfAbsent(userIdProperty);
        }
        // neni instance uzivatele, zkus se kouknout do http hlavicek
        else if (hasKey(SecurityConstants.ContextUserHttpHeaderUserIdKey))
        {
            var userIdProperty = propertyFactory.CreateProperty(Constants.LoggerContextUserIdPropertyName, _contextAccessor.HttpContext!.Request.Headers[SecurityConstants.ContextUserHttpHeaderUserIdKey].First());
            logEvent.AddPropertyIfAbsent(userIdProperty);

            if (hasKey(SecurityConstants.ContextUserHttpHeaderUserIdentKey))
            {
                var userIdentProperty = propertyFactory.CreateProperty(Constants.LoggerContextUserIdentPropertyName, _contextAccessor.HttpContext!.Request.Headers[SecurityConstants.ContextUserHttpHeaderUserIdentKey].First());
                logEvent.AddPropertyIfAbsent(userIdentProperty);
            }
        }
        // posledni pokus - muze byt jiz vytvorena claims identity, ale jeste neni v kontextu User z auth middlewaru
        else if (_contextAccessor.HttpContext?.User?.HasClaim(t => t.Type == SecurityConstants.ClaimTypeIdent) ?? false)
        {
            var userIdentProperty = propertyFactory.CreateProperty(Constants.LoggerContextUserIdentPropertyName, _contextAccessor.HttpContext!.User.Claims.First(t => t.Type == SecurityConstants.ContextUserHttpHeaderUserIdentKey).Value);
            logEvent.AddPropertyIfAbsent(userIdentProperty);
        }
    }

    private bool hasKey(string key)
    {
        return _contextAccessor.HttpContext?.Request?.Headers?.ContainsKey(key) ?? false;
    }
}
