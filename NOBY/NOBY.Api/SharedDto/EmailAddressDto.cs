namespace NOBY.Api.SharedDto;

public sealed class EmailAddressDto : IEmailAddressDto
{
    public string? EmailAddress { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public bool IsConfirmed { get; set; }
}

public interface IEmailAddressDto
{
    string? EmailAddress { get; set; }

    bool IsConfirmed { get; set; }
}