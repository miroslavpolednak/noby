using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CIS.Security.InternalServices
{
    public class CisUserContextHelpers : ICisUserContextHelpers
    {
        public const string ContextUserBaggageKey = "MpPartyId";
        private readonly IHttpContextAccessor _context;

        public CisUserContextHelpers(IHttpContextAccessor context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context), "IHttpContextAccessor is not registered in DI container, use services.AddHttpContextAccessor()");

            _context = context;
        }

        public async Task<TResult> AddUserContext<TResult>(Func<Task<TResult>> serviceCall)
        {
            var userId = Activity.Current?.Baggage.FirstOrDefault(b => b.Key == ContextUserBaggageKey).Value;
            if (string.IsNullOrEmpty(userId))
                userId = _context?.HttpContext?.User?.Identity?.Name;

            if (!string.IsNullOrEmpty(userId))
            {
                var activity = new Activity("CisContextUser")
                    .AddBaggage(ContextUserBaggageKey, userId)
                    .Start();

                var result = await serviceCall();

                activity.Stop();

                return result;
            }
            else
                return await serviceCall();
        }

        public async Task AddUserContext(Func<Task> serviceCall)
        {
            var userId = Activity.Current?.Baggage.FirstOrDefault(b => b.Key == ContextUserBaggageKey).Value;
            if (string.IsNullOrEmpty(userId))
                userId = _context?.HttpContext?.User?.Identity?.Name;

            if (!string.IsNullOrEmpty(userId))
            {
                var activity = new Activity("CisContextUser")
                    .AddBaggage(ContextUserBaggageKey, userId)
                    .Start();

                await serviceCall();

                activity.Stop();
            }
            else
                await serviceCall();
        }
    }
}
