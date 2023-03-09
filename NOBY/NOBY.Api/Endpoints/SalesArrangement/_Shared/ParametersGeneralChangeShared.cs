using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.SalesArrangement.Dto;

/// <summary>
/// Zajištění
/// </summary>
public sealed class Collateral
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    [Required]
    public bool IsActive { get; set; }

    /// <summary>
    /// Identifikace nemovitosti (přidat zajištění)
    /// </summary>
    public string? AddLoanRealEstateCollateral { get; set; }

    /// <summary>
    /// Identifikace nemovitosti (uvolnit zajištění)
    /// </summary>
    public string? ReleaseLoanRealEstateCollateral { get; set; }

}

public sealed class LoanRealEstate
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    [Required]
    public bool IsActive { get; set; }

    public List<LoanRealEstateItem>? LoanRealEstates { get; set; }
}

/// <summary>
/// Objekt úvěru
/// </summary>
public sealed class LoanRealEstateItem
{
    /// <summary>
    /// Typ nemovitosti
    /// </summary>
    public long RealEstateTypeId { get; set; }

    /// <summary>
    /// Účel pořízení nemovitosti
    /// </summary>
    public int RealEstatePurchaseTypeId { get; set; }
}

/// <summary>
/// Účel úvěru
/// </summary>
public sealed class LoanPurpose
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    [Required]
    public bool IsActive { get; set; }

    /// <summary>
    /// Komentář k popisu změny na stávajících účelech úvěru
    /// </summary>
    public string? LoanPurposesComment { get; set; }
}

/// <summary>
/// Podmínky čerpání a další podmínky
/// </summary>
public sealed class DrawingAndOtherConditions
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    [Required]
    public bool IsActive { get; set; }

    /// <summary>
    /// Komentář ke změně v podmínkách smlouvy
    /// </summary>
    public string? CommentToChangeContractConditions { get; set; }

}

/// <summary>
/// Komentář k žádosti o změnu
/// </summary>
public sealed class CommentToChangeRequest
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    [Required]
    public bool IsActive { get; set; }

    /// <summary>
    /// Obecný komentář
    /// </summary>
    public string? GeneralComment { get; set; }

}
