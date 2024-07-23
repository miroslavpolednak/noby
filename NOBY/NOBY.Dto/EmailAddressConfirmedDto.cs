namespace NOBY.Dto;

public sealed class EmailAddressConfirmedDto : IEmailAddressDto
{
    /// <summary>
    /// Emailová adresa
    /// </summary>
    public string EmailAddress { get; set; } = string.Empty;

    /// <summary>
    /// Příznak potvrzeného kontaktu
    /// </summary>
    public bool IsConfirmed { get; set; }
}
