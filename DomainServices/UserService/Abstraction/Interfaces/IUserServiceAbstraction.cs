using CIS.Core.Results;

namespace DomainServices.UserService.Abstraction;

public interface IUserServiceAbstraction
{
    /// <summary>
    /// Vraci detail uzivatele
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[int (Contracts.User)] - OK;
    /// </returns>
    Task<IServiceCallResult> GetUserByLogin(string login, CancellationToken cancellationToken = default(CancellationToken));
}
