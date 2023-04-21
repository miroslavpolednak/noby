using CIS.Foms.Types;
using NOBY.Api.Endpoints.SalesArrangement.Dto;
using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.SalesArrangement.UpdateParameters.Dto;

public sealed class HUBNUpdate
{
    /// <summary>
    /// Identita klienta
    /// </summary>
    public CustomerIdentity? Applicant { get; set; }

    /// <summary>
    /// Výše úvěru
    /// </summary>
    [Required]
    public LoanAmount LoanAmount { get; set; } = null!;

    /// <summary>
    /// Účely úvěru
    /// </summary>
    [Required]
    public List<LoanPurposeItem>? LoanPurposes { get; set; }

    /// <summary>
    /// Objekty úvěru
    /// </summary>
    [Required]
    public List<LoanRealEstateItemExtended>? LoanRealEstates { get; set; }

    /// <summary>
    /// Identifikace zajištění
    /// </summary>
    [Required]
    public CollateralIdentification CollateralIdentification { get; set; } = null!;

    /// <summary>
    /// Předpokládaný termín prvního čerpání
    /// </summary>
    [Required]
    public ExpectedDateOfDrawing ExpectedDateOfDrawing { get; set; } = null!;

    /// <summary>
    /// Lhůta ukončení čerpání
    /// </summary>
    [Required]
    public DrawingDateTo DrawingDateTo { get; set; } = null!;

    /// <summary>
    /// Komentář k žádosti o změnu
    /// </summary>
    [Required]
    public CommentToChangeRequest CommentToChangeRequest { get; set; } = null!;
}

public sealed class LoanAmount
{
    /// <summary>
    /// Změnit sjednanou výši úvěru
    /// </summary>
    [Required]
    public bool ChangeAgreedLoanAmount { get; set; }

    /// <summary>
    /// Požadovaná výše úvěru
    /// </summary>
    public decimal? RequiredLoanAmount { get; set; }

    /// <summary>
    /// Zachovat sjednanou splatnost do
    /// </summary>
    [Required]
    public bool PreserveLoanDueDate { get; set; }

    /// <summary>
    /// Zachovat sjednanou splátku
    /// </summary>
    [Required]
    public bool PreserveAgreedPaymentAmount { get; set; }
}

public sealed class ExpectedDateOfDrawing
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    [Required]
    public bool IsActive { get; set; }

    /// <summary>
    /// Nový předpokládaný termín prvního čerpání
    /// </summary>
    public DateTime? NewExpectedDateOfDrawing { get; set; }
}

public sealed class DrawingDateTo
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    [Required]
    public bool IsActive { get; set; }

    /// <summary>
    /// Prodloužení konce lhůty čerpání o kolik měsíců
    /// </summary>
    public int? ExtensionDrawingDateToByMonths { get; set; }
}