namespace NOBY.Api.SharedDto;

public sealed class ConfirmedEmailAddressDto
{
    public string? EmailAddress { get; set; }

    public bool IsConfirmed { get; set; }
}
