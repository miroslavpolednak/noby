using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.CustomerService.ExternalServices.Contacts.V1;

public interface IContactClient: IExternalServiceClient
{
    const string Version = "V1";
    Task<Contracts.ValidateContactResponse> ValidatePhone(string phoneNumber, CancellationToken cancellationToken = default);
    Task<Contracts.ValidateContactResponse> ValidateEmail(string emailAddress, CancellationToken cancellationToken = default);
}