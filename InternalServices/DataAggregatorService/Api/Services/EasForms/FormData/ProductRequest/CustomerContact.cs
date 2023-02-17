using DomainServices.CustomerService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData.ProductRequest;

internal class CustomerContact
{
    private readonly Contact _contact;

    public CustomerContact(Contact contact)
    {
        _contact = contact;
    }

    public int ContactTypeId => _contact.ContactTypeId;

    public string Value
    {
        get
        {
            return _contact.DataCase switch
            {
                Contact.DataOneofCase.Mobile => (_contact.Mobile.PhoneIDC + _contact.Mobile.PhoneNumber).Trim(),
                Contact.DataOneofCase.Email => _contact.Email.Address,
                _ => string.Empty
            };
        }
    }
}