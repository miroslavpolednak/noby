namespace CIS.Security.InternalServices;

public interface ICisUserContextHelpers
{
    Task<TResult> AddUserContext<TResult>(Func<Task<TResult>> serviceCall);

    Task AddUserContext(Func<Task> serviceCall);
}
