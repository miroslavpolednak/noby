using CIS.Core.Security;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace CIS.Infrastructure.Audit;

internal sealed class AuditLogger
    : IAuditLogger
{
    public void Log(
        AuditEventTypes eventType,
        string message,
        ICollection<AuditLoggerHeaderItem>? identities = null,
        ICollection<AuditLoggerHeaderItem>? products = null,
        AuditLoggerHeaderItem? operation = null,
        string? result = null,
        IDictionary<string, string>? bodyBefore = null,
        IDictionary<string, string>? bodyAfter = null)
    {
        var user = Telemetry.Helpers.GetCurrentUser(_currentUser, _contextAccessor);

        var context = new Dto.AuditEventContext()
        {
            EventType = eventType,
            Message = message,
            Result = result,
            Identities = identities,
            Products = products,
            Operation = operation,
            BodyAfter = bodyAfter,
            BodyBefore = bodyBefore,
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
    private readonly IAuditLoggerInternal _helper;

    public AuditLogger(IAuditLoggerInternal helper, IHttpContextAccessor contextAccessor, ICurrentUserAccessor currentUser)
    {
        _helper = helper;
        _contextAccessor = contextAccessor;
        _currentUser = currentUser;
    }
}