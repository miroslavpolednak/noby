namespace NOBY.Api.SharedDto;

public sealed class ConfirmedContactsDto
{
    public ConfirmedEmailAddressDto? EmailAddress { get; set; }

    public ConfirmedPhoneNumberDto? PhoneNumber { get; set; }
}
