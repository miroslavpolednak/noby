namespace NOBY.Api.SharedDto;

public sealed class ContactsConfirmedDto
{
    public EmailAddressConfirmedDto? EmailAddress { get; set; }

    public PhoneNumberConfirmedDto? MobilePhone { get; set; }
}
