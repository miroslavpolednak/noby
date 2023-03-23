namespace NOBY.Api.Endpoints.Cases.GetCaseParameters.Dto;

public sealed class StatementDto
{
    /// <summary>
    /// Způsob odběru výpisů
    /// </summary>
    /// <example>elektronicky</example>
    public int? Type { get; set; }

    /// <summary>
    /// Frekvence výpisů
    /// </summary>
    /// <example>měsíční</example>
    public int? Frequency { get; set; }

    /// <summary>
    /// Emailová adresa 1
    /// </summary>
    /// <remarks>alda.rajcan@volny.cz</remarks>
    public string? EmailAddress1 { get; set; }

    /// <summary>
    /// Emailová adresa 2
    /// </summary>
    /// <remarks>alda.rajcan@volny.cz</remarks>
    public string? EmailAddress2 { get; set; }

    /// <summary>
    /// Adresa - bydliste, kontaktni atd.
    /// </summary>
    public CIS.Foms.Types.Address? Address { get; set; }
}
