namespace CIS.DomainServicesSecurity;

public interface ILoginValidator
{
    Task<bool> Validate(string login, string password);
}
