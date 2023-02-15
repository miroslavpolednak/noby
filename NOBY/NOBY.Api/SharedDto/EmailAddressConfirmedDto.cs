namespace NOBY.Api.SharedDto;

public sealed class EmailAddressConfirmedDto : IEmailAddressDto
{
    public string? EmailAddress { get; set; }

    public bool IsConfirmed { get; set; }
}
