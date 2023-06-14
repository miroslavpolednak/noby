using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.CustomerService.ExternalServices.Contacts.V1;

public interface IContactClient: IExternalServiceClient
{
    Task ValidatePhone(CancellationToken cancellationToken = default);
    Task ValidateEmail(CancellationToken cancellationToken = default);
}