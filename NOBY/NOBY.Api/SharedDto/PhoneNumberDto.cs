namespace NOBY.Api.SharedDto;

public sealed class PhoneNumberDto : IPhoneNumberDto
{
    /// <summary>
    /// Telefonní číslo
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Předvolba telefonního čísla
    /// </summary>
    public string? PhoneIDC { get; set; }

    /// <summary>
    /// Příznak potvrzeného kontaktu
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public bool IsConfirmed { get; set; }
}

public interface IPhoneNumberDto
{
    /// <summary>
    /// Telefonní číslo
    /// </summary>
    string? PhoneNumber { get; set; }

    /// <summary>
    /// Předvolba telefonního čísla
    /// </summary>
    string? PhoneIDC { get; set; }

    /// <summary>
    /// Příznak potvrzeného kontaktu
    /// </summary>
    bool IsConfirmed { get; set; }
}