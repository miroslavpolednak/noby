namespace DomainServices.UserService.Clients;

public interface IUserServiceClient
{
    Task<Contracts.User> GetUser(string id, string schema, CancellationToken cancellationToken = default(CancellationToken));
}
