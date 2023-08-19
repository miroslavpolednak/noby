namespace NOBY.Dto.Workflow;

/// <summary>
/// Tento objekt je relevantní pouze pro typ úkolu Podepisování
/// </summary>
public class AmendmentsSigning
{
    /// <summary>
    /// Podepisování: ID čárového kódu dokumentu k podpisu (formId)
    /// </summary>
    /// <example>S00000014139920</example>
    [Required]
    public string FormId { get; set; } = null!;

    /// <summary>
    /// Metoda podpisu (manuální/elektronický). Číselník SignatureType.
    /// </summary>
    /// <example>1</example>
    public int? SignatureTypeId { get; set; }
    
    /// <summary>
    /// Podepisování: Lhůta pro zajištění podpisu
    /// </summary>
    /// <example>24.12.2023</example>
    [Required]
    [MinLength(1)]
    public DateOnly Expiration { get; set; }


    /// <summary>
    /// Podepisování: ID dokumentu k podpisu
    /// </summary>
    /// <example>01_23_046_111203_SF_0001</example>
    [Required]
    public string DocumentForSigning { get; set; } = null!;

    /// <summary>
    /// Podepisování: Typ dokumentu k podpisu. D pro úvěrovou smlouvu, A pro zástavní smlouvu.
    /// </summary>
    /// <example>A</example>
    [Required]
    [MinLength(1)]
    [MaxLength(1)]
    public string DocumentForSigningType { get; set; } = null!;
    
    /// <summary>
    /// Podepisování: ID dokumentu návrhu na vklad
    /// </summary>
    /// <example>01_23_046_111203_SF_0001</example>
    public string ProposalForEntry { get; set; } = null!;
}