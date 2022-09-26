using CIS.Core.Results;

namespace DomainServices.UserService.Clients;

public interface IUserServiceClient
{
    /// <summary>
    /// Vraci detail uzivatele
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[int (Contracts.User)] - OK;
    /// </returns>
    Task<IServiceCallResult> GetUserByLogin(string login, CancellationToken cancellationToken = default(CancellationToken));

    Task<IServiceCallResult> GetUser(int userId, CancellationToken cancellationToken = default(CancellationToken));
}
