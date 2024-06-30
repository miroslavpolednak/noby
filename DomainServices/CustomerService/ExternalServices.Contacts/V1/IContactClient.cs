using CIS.Infrastructure.ExternalServicesHelpers;
using DomainServices.CustomerService.ExternalServices.Contacts.V1.Contracts;

namespace DomainServices.CustomerService.ExternalServices.Contacts.V1;

public interface IContactClient: IExternalServiceClient
{
    const string Version = "V1";
    Task<Contracts.ValidateContactResponse> ValidatePhone(string phoneNumber, CancellationToken cancellationToken = default);
    Task<Contracts.ValidateContactResponse> ValidateEmail(string emailAddress, CancellationToken cancellationToken = default);
    Task<List<Contact>> LoadContacts(long customerId, CancellationToken cancellationToken = default);

    Task CreateOrUpdateContact(long customerId, int contactMethodCode, string contactValue, CancellationToken cancellationToken = default);

}