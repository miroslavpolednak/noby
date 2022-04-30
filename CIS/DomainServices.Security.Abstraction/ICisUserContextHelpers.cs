namespace CIS.DomainServices.Security.Abstraction;

public interface ICisUserContextHelpers
{
    Task<TResult> AddUserContext<TResult>(Func<Task<TResult>> serviceCall);

    Task AddUserContext(Func<Task> serviceCall);
}
