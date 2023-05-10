using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.Cases.GetTaskDetail.Dto.Amendments;

/// <summary>
/// Tento objekt je relevantní pouze pro typ úkolu Podepisování
/// </summary>
public class AmendmentsSigning : Amendments
{
    /// <summary>
    /// Podepisování: ID čárového kódu dokumentu k podpisu (formId)
    /// </summary>
    /// <example>S00000014139920</example>
    [Required]
    public string FormId { get; set; } = null!;

    /// <summary>
    /// Podepisování: způsob podpisu (0 - papírové podepisování, 1 - elektronické podepisování)
    /// </summary>
    /// <example>0</example>
    public int SignatureType { get; set; }
    

    /// <summary>
    /// Podepisování: Lhůta pro zajištění podpisu
    /// </summary>
    /// <example>24.12.2023</example>
    [Required]
    public DateTime Expiration { get; set; }
    

    /// <summary>
    /// Podepisování: ID dokumentu k podpisu
    /// </summary>
    /// <example>01_23_046_111203_SF_0001</example>
    [Required]
    public string DocumentForSigning { get; set; } = null!;

    /// <summary>
    /// Podepisování: ID dokumentu návrhu na vklad
    /// </summary>
    /// <example>01_23_046_111203_SF_0001</example>
    public string ProposalForEntry { get; set; } = null!;
}