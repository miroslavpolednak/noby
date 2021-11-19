using CIS.Security.InternalServices.Identities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CIS.Security.InternalServices
{
    public sealed class CisUserContextMiddleware
    {
        private readonly ILogger<CisUserContextMiddleware> _logger;
        private readonly RequestDelegate _next;

        public CisUserContextMiddleware(RequestDelegate next, ILoggerFactory logFactory)
        {
            _logger = logFactory.CreateLogger<CisUserContextMiddleware>();
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var partyId = Activity.Current?.Baggage.FirstOrDefault(b => b.Key == "MpPartyId").Value;
            if (!string.IsNullOrEmpty(partyId))
            {
                _logger.LogDebug("Context user identity {PartyId} added", partyId);

                // vytvorit identity
                var identity = new CisUserContextIdentity(partyId);
                if (httpContext.User != null)
                    httpContext.User.AddIdentity(identity);
            }

            await _next(httpContext);
        }
    }
}
