namespace NOBY.Api.SharedDto;

public sealed class EmailAddressConfirmedDto
{
    public string? EmailAddress { get; set; }

    public bool IsConfirmed { get; set; }
}
