namespace NOBY.Api.SharedDto;

public sealed class PhoneNumberDto : IPhoneNumberDto
{
    public string? PhoneNumber { get; set; }

    public string? PhoneIDC { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public bool IsConfirmed { get; set; }
}

public interface IPhoneNumberDto
{
    string? PhoneNumber { get; set; }

    string? PhoneIDC { get; set; }

    bool IsConfirmed { get; set; }
}