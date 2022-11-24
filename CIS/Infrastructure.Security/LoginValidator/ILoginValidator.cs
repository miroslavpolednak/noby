namespace CIS.Infrastructure.Security;

public interface ILoginValidator
{
    Task<bool> Validate(string login, string password);
}
