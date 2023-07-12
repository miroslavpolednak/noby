using CIS.Core.Security;
using Microsoft.AspNetCore.Http;

namespace CIS.Infrastructure.Telemetry.AuditLog;

internal sealed class AuditLogger
    : IAuditLogger
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly AuditLoggerHelper _helper;

    public AuditLogger(AuditLoggerHelper helper, IHttpContextAccessor contextAccessor, ICurrentUserAccessor currentUser)
    {
        _helper = helper;
        _contextAccessor = contextAccessor;
        _currentUser = currentUser;
    }

    public void Log(AuditEventTypes eventType)
    {
        var headers = new AuditEventHeaders(getIpAddress());
        _helper.Log(eventType, ref headers);
    }

    private string getIpAddress()
    {
        string? proxyIps = _contextAccessor.HttpContext?.Request?.Headers?["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(proxyIps))
        {
            int idx = proxyIps.IndexOf(',', 1);
            if (idx > 0) return proxyIps[..idx];
        }
        return _contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "";
    }
}
