using CIS.Foms.Types;
using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.SalesArrangement.Dto;

public sealed class ParametersHUBN
{
    /// <summary>
    /// Identita klienta
    /// </summary>
    public CustomerIdentity? Applicant { get; set; }

    /// <summary>
    /// Výše úvěru
    /// </summary>
    [Required]
    public LoanAmountObject LoanAmount { get; set; }

    /// <summary>
    /// Účely úvěru
    /// </summary>
    [Required]
    public List<LoanPurposeItem> LoanPurposes { get; set; }

    /// <summary>
    /// Objekty úvěru
    /// </summary>
    [Required]
    public List<LoanRealEstateItem2> LoanRealEstates { get; set; }

    /// <summary>
    /// Identifikace zajištění
    /// </summary>
    [Required]
    public CollateralIdentificationObject CollateralIdentification { get; set; }

    /// <summary>
    /// Předpokládaný termín prvního čerpání
    /// </summary>
    [Required]
    public ExpectedDateOfDrawingObject ExpectedDateOfDrawing { get; set; }

    /// <summary>
    /// Lhůta ukončení čerpání
    /// </summary>
    [Required]
    public DrawingDateToObject2 DrawingDateTo { get; set; }

    /// <summary>
    /// Komentář k žádosti o změnu
    /// </summary>
    [Required]
    public CommentToChangeRequestObject CommentToChangeRequest { get; set; }
}

/// <summary>
/// Účet pro splácení
/// </summary>
public sealed class PaymentAccountObject2
{
    /// <summary>
    /// Předčíslí účtu
    /// </summary>
    public string AgreedPrefix { get; set; }

    /// <summary>
    /// Číslo účtu
    /// </summary>
    public string AgreedNumber { get; set; }

    /// <summary>
    /// Kód banky
    /// </summary>
    public string AgreedBankCode { get; set; }
}

public sealed class LoanAmountObject
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

/// <summary>
/// Účel úvěru
/// </summary>
public sealed class LoanPurposeItem
{
    /// <summary>
    /// Účel úvěru
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Výše úvěru pro zvolený účel v Kč
    /// </summary>
    public decimal Sum { get; set; }
}

public sealed class LoanRealEstateItem2
{
    /// <summary>
    /// Typ nemovitosti
    /// </summary>
    public int RealEstateTypeId { get; set; }

    /// <summary>
    /// Slouží k zajištění
    /// </summary>
    public bool IsCollateral { get; set; }

    /// <summary>
    /// Účel pořízení nemovitosti
    /// </summary>
    public int RealEstatePurchaseTypeId { get; set; }
}

public sealed class CollateralIdentificationObject
{
    /// <summary>
    /// Identifikace nemovitosti
    /// </summary>
    public string RealEstateIdentification { get; set; }
}

public sealed class ExpectedDateOfDrawingObject
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

public sealed class DrawingDateToObject2
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    [Required]
    public bool IsActive { get; set; }

    /// <summary>
    /// Prodloužení konce lhůty čerpání o kolik měsíců
    /// </summary>
    public int ExtensionDrawingDateToByMonths { get; set; }
}