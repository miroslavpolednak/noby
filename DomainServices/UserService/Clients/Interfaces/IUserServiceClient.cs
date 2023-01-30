namespace DomainServices.UserService.Clients;

public interface IUserServiceClient
{
    /// <summary>
    /// Vraci detail uzivatele
    /// </summary>
    Task<Contracts.User> GetUserByLogin(string login, CancellationToken cancellationToken = default(CancellationToken));

    Task<Contracts.User> GetUser(int userId, CancellationToken cancellationToken = default(CancellationToken));
}
