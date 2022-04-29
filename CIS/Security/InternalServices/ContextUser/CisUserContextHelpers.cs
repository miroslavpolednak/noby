using System.Diagnostics;

namespace CIS.Security.InternalServices.ContextUser;

public sealed class CisUserContextHelpers 
    : ICisUserContextHelpers
{
    public const string ContextUserBaggageKey = "MpPartyId";

    private readonly Core.Security.ICurrentUserAccessor _userAccessor;

    public CisUserContextHelpers(Core.Security.ICurrentUserAccessor userAccessor)
    {
        if (userAccessor is null)
            throw new ArgumentNullException(nameof(userAccessor), "ICurrentUserAccessor is not registered in DI container");

        _userAccessor = userAccessor;
    }

    public async Task<TResult> AddUserContext<TResult>(Func<Task<TResult>> serviceCall)
    {
        int? userId = getUserId();
        if (userId.HasValue)
        {
            var activity = new Activity("CisContextUser")
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
            var activity = new Activity("CisContextUser")
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
            return _userAccessor.User?.Id;
        else
            return i;
    }
}
