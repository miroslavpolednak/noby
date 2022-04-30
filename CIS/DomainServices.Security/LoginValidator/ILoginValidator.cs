namespace CIS.DomainServices.Security;

public interface ILoginValidator
{
    Task<bool> Validate(string login, string password);
}
