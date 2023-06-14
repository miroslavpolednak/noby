namespace DomainServices.CustomerService.ExternalServices.Contacts.V1;

public class RealContactClient : IContactClient
{
    public Task ValidatePhone(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task ValidateEmail(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}