using CIS.Core.Security;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using static CIS.Infrastructure.Telemetry.AuditLog.AuditLogger;

namespace CIS.Infrastructure.Telemetry.AuditLog;

internal sealed class AuditLogger
    : IAuditLogger
{
    public void Log(
        AuditEventTypes eventType,
        ICollection<AuditLoggerHeaderItem>? identities = null,
        ICollection<AuditLoggerHeaderItem>? products = null,
        AuditLoggerHeaderItem? operation = null)
    {
        var user = Helpers.GetCurrentUser(_currentUser, _contextAccessor);

        var context = new AuditEventContext()
        {
            EventType = eventType,
            Identities = identities,
            Products = products,
            Operation = operation,
            ClientIp = getIpAddress(),
            UserIdent = user.UserIdent,
            Correlation = Activity.Current?.Id
        };
        _helper.Log(context);
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

    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly AuditLoggerHelper _helper;

    public AuditLogger(AuditLoggerHelper helper, IHttpContextAccessor contextAccessor, ICurrentUserAccessor currentUser)
    {
        _helper = helper;
        _contextAccessor = contextAccessor;
        _currentUser = currentUser;
    }
}