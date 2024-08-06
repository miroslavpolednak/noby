using DomainServices.CustomerService.ExternalServices.Contacts.V1;

namespace DomainServices.CustomerService.Api.Services.CustomerManagement;

[ScopedService, SelfService]
public class CustomerManagementContactService(IContactClient contactClient)
{
    public async Task CreateEmail(long customerId, IEnumerable<Contact> contacts, CancellationToken cancellationToken = default)
    {
        var email = contacts.FirstOrDefault(c => c.ContactTypeId == (int)ContactTypes.Email);

        if (email is null)
            return;

        await contactClient.CreateOrUpdateContact(customerId, 15, email.Email.EmailAddress, cancellationToken);
        await contactClient.CreateOrUpdateContact(customerId, 7, email.Email.EmailAddress, cancellationToken);
    }

    public async Task UpdateEmail(long customerId, IEnumerable<Contact> contacts, CancellationToken cancellationToken = default)
    {
        var email = contacts.FirstOrDefault(c => c.ContactTypeId == (int)ContactTypes.Email);

        if (email is null)
            return;

        var cmContacts = await contactClient.LoadContacts(customerId, cancellationToken);

        if (cmContacts.Any(c => c is { ContactMethodCode: 7, Confirmed: true }))
            return;

        await contactClient.CreateOrUpdateContact(customerId, 15, email.Email.EmailAddress, cancellationToken);
        await contactClient.CreateOrUpdateContact(customerId, 7, email.Email.EmailAddress, cancellationToken);
    }
}