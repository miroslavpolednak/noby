namespace NOBY.Api.Endpoints.Cases.GetCaseParameters.Dto;

public sealed class StatementDto
{
    [Obsolete]
    public string? Type { get; set; }

    /// <summary>
    /// Id typu výpisů
    /// </summary>
    /// <example>2</example>
    public int? TypeId { get; set; }

    /// <summary>
    /// Typ výpisů
    /// </summary>
    /// <example>Elektronický</example>
    public string? TypeShortName { get; set; }
    
    /// <summary>
    /// Způsob odběru výpisů
    /// </summary>
    /// <example>elektronicky</example>
    [Obsolete]
    public string? SubscriptionType { get; set; }

    /// <summary>
    /// Frekvence výpisů
    /// </summary>
    /// <example>měsíční</example>
    public string? Frequency { get; set; }

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
