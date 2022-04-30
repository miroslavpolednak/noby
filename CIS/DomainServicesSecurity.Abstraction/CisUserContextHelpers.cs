using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.DomainServicesSecurity.Abstraction;

internal sealed class CisUserContextHelpers 
    : ICisUserContextHelpers
{
    const string ActivityName = "CisContextUser";
    const string ContextUserBaggageKey = "MpPartyId";

    private readonly Core.Security.ICurrentUserAccessor? _userAccessor;

    public CisUserContextHelpers(IHttpContextAccessor httpContext)
    {
        _userAccessor = httpContext.HttpContext?.RequestServices.GetServices<Core.Security.ICurrentUserAccessor>().FirstOrDefault();
    }

    public async Task<TResult> AddUserContext<TResult>(Func<Task<TResult>> serviceCall)
    {
        int? userId = getUserId();
        if (userId.HasValue)
        {
            var activity = new Activity(ActivityName)
                .AddBaggage(ContextUserBaggageKey, userId.ToString())
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
        int? userId = getUserId();
        if (userId.HasValue)
        {
            var activity = new Activity(ActivityName)
                .AddBaggage(ContextUserBaggageKey, userId.ToString())
                .Start();

            await serviceCall();

            activity.Stop();
        }
        else
            await serviceCall();
    }

    private int? getUserId()
    {
        if (!int.TryParse(Activity.Current?.Baggage.FirstOrDefault(b => b.Key == ContextUserBaggageKey).Value, out int i))
            return _userAccessor?.User?.Id;
        else
            return i;
    }
}
