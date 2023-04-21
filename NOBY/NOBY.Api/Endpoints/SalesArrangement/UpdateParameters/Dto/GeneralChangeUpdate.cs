using CIS.Foms.Types;
using NOBY.Api.Endpoints.SalesArrangement.Dto;
using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.SalesArrangement.UpdateParameters.Dto;

public sealed class GeneralChangeUpdate
{
    /// <summary>
    /// Identita klienta
    /// </summary>
    public CustomerIdentity? Applicant { get; set; }

    /// <summary>
    /// Zajištění
    /// </summary>
    [Required]
    public Collateral Collateral { get; set; } = null!;

    /// <summary>
    /// Den splácení
    /// </summary>
    [Required]
    public PaymentDay PaymentDay { get; set; } = null!;

    /// <summary>
    /// Lhůta ukončení čerpání
    /// </summary>
    [Required]
    public DrawingDateToExtended DrawingDateTo { get; set; } = null!;

    /// <summary>
    /// Účet pro splácení
    /// </summary>
    [Required]
    public PaymentAccount RepaymentAccount { get; set; } = null!;

    /// <summary>
    /// Výše měsíční splátky
    /// </summary>
    [Required]
    public LoanPaymentAmount LoanPaymentAmount { get; set; } = null!;

    /// <summary>
    /// Splatnost
    /// </summary>
    [Required]
    public DueDate DueDate { get; set; } = null!;

    /// <summary>
    /// Objekty úvěru
    /// </summary>
    [Required]
    public LoanRealEstate LoanRealEstate { get; set; } = null!;

    /// <summary>
    /// Účel úvěru
    /// </summary>
    [Required]
    public LoanPurpose LoanPurpose { get; set; } = null!;

    /// <summary>
    /// Podmínky čerpání a další podmínky
    /// </summary>
    [Required]
    public DrawingAndOtherConditions DrawingAndOtherConditions { get; set; } = null!;

    /// <summary>
    /// Komentář k žádosti o změnu
    /// </summary>
    [Required]
    public CommentToChangeRequest CommentToChangeRequest { get; set; } = null!;
}

/// <summary>
/// Den splácení
/// </summary>
public sealed class PaymentDay
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    [Required]
    public bool IsActive { get; set; }

    /// <summary>
    /// Nový den splácení, CIS_DEN_SPLACENI
    /// </summary>
    public int? NewPaymentDay { get; set; }
}

/// <summary>
/// Lhůta ukončení čerpání
/// </summary>
public sealed class DrawingDateToExtended
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

    /// <summary>
    /// Komentář ke lhůtě ukončení čerpání
    /// </summary>
    public string? CommentToDrawingDateTo { get; set; }

}

/// <summary>
/// Účet pro splácení
/// </summary>
public sealed class PaymentAccount
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    [Required]
    public bool IsActive { get; set; }

    /// <summary>
    /// Předčíslí účtu
    /// </summary>
    public string? Prefix { get; set; }

    /// <summary>
    /// Číslo účtu
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// Kód banky
    /// </summary>
    public string? BankCode { get; set; }

    /// <summary>
    /// Jméno majitele účtu
    /// </summary>
    public string? OwnerFirstName { get; set; }

    /// <summary>
    /// Příjmení majitele účtu
    /// </summary>
    public string? OwnerLastName { get; set; }

    /// <summary>
    /// Datum narození majitele účtu
    /// </summary>
    public DateTime? OwnerDateOfBirth { get; set; }
}

/// <summary>
/// Výše měsíční splátky
/// </summary>
public sealed class LoanPaymentAmount
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    [Required]
    public bool IsActive { get; set; }

    /// <summary>
    /// Nová výše měsíční splátky
    /// </summary>
    public decimal? NewLoanPaymentAmount { get; set; }

    /// <summary>
    /// V souvislosti s mimořádnou splátkou
    /// </summary>
    [Required]
    public bool ConnectionExtraordinaryPayment { get; set; }
}

/// <summary>
/// Splatnost
/// </summary>
public sealed class DueDate
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Nový datum splatnosti
    /// </summary>
    public DateTime? NewLoanDueDate { get; set; }

    /// <summary>
    /// V souvislosti s mimořádnou splátkou
    /// </summary>
    [Required]
    public bool ConnectionExtraordinaryPayment { get; set; }
}
