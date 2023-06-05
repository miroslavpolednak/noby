namespace NOBY.Dto;

public sealed class PhoneNumberConfirmedDto : IPhoneNumberDto
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
    public bool IsConfirmed { get; set; }
}
