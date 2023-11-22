namespace NOBY.Dto;

public sealed class ContactsDto
{
    [EmailAddress]
    public EmailAddressDto? EmailAddress { get; set; }

    public PhoneNumberDto? MobilePhone { get; set; }
}
