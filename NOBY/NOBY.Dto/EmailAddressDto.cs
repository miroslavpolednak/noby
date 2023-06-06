namespace NOBY.Dto;

public sealed class EmailAddressDto : IEmailAddressDto
{
    /// <summary>
    /// Emailová adresa
    /// </summary>
    public string? EmailAddress { get; set; }

    /// <summary>
    /// Příznak potvrzeného kontaktu
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public bool IsConfirmed { get; set; }
}

public interface IEmailAddressDto
{
    /// <summary>
    /// Emailová adresa
    /// </summary>
    string? EmailAddress { get; set; }

    /// <summary>
    /// Příznak potvrzeného kontaktu
    /// </summary>
    bool IsConfirmed { get; set; }
}