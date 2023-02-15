namespace NOBY.Api.SharedDto;

public sealed class PhoneNumberConfirmedDto
{
    public string? PhoneNumber { get; set; }

    public string? PhoneIDC { get; set; }

    public bool IsConfirmed { get; set; }
}
