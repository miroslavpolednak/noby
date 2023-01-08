using CIS.Foms.Types;
using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.SalesArrangement.Dto;

public sealed class ParametersGeneralChange
{
    public CustomerIdentity? Applicant { get; set; }

    [Required]
    public CollateralObject Collateral { get; set; }

    [Required]
    public PaymentDayObject PaymentDay { get; set; }

    [Required]
    public DrawingDateToObject DrawingDateTo { get; set; }

    [Required]
    public PaymentAccountObject PaymentAccount { get; set; }

    [Required]
    public LoanPaymentAmountObject LoanPaymentAmount { get; set; }

    [Required]
    public DueDateObject DueDate { get; set; }

    /// <summary>
    /// Objekty úvěru
    /// </summary>
    [Required]
    public LoanRealEstateObject LoanRealEstates { get; set; }

    [Required]
    public LoanPurposeObject LoanPurpose { get; set; }

    [Required]
    public DrawingAndOtherConditionsObject DrawingAndOtherConditions { get; set; }

    [Required]
    public CommentToChangeRequestObject CommentToChangeRequest { get; set; }
}

public sealed class LoanRealEstateObject
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    public bool IsActive { get; set; }

    public List<LoanRealEstateItem> LoanRealEstates { get; set; }
}

public sealed class LoanRealEstateItem
{
    public int RealEstateTypeId { get; set; }

    public int RealEstatePurchaseTypeId { get; set; }
}

/// <summary>
/// Zajištění
/// </summary>
public sealed class CollateralObject
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
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

/// <summary>
/// Den splácení
/// </summary>
public sealed class PaymentDayObject
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Sjednaný den splácení
    /// </summary>
    public int AgreedPaymentDay { get; set; }

    /// <summary>
    /// Nový den splácení, CIS_DEN_SPLACENI
    /// </summary>
    public int? NewPaymentDay { get; set; }
}

/// <summary>
/// Lhůta ukončení čerpání
/// </summary>
public sealed class DrawingDateToObject
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Sjednaný termín čerpání do
    /// </summary>
    public DateTime AgreedDrawingDateTo { get; set; }

    /// <summary>
    /// Prodloužení konce lhůty čerpání o kolik měsíců
    /// </summary>
    public int? ExtensionByMonths { get; set; }

    /// <summary>
    /// Komentář ke lhůtě ukončení čerpání
    /// </summary>
    public string? CommentToDrawingDateTo { get; set; }

}

/// <summary>
/// Účet pro splácení
/// </summary>
public sealed class PaymentAccountObject
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    public bool IsActive { get; set; }

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
public sealed class LoanPaymentAmountObject
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Aktuální výše měsíční splátky
    /// </summary>
    public decimal ActualLoanPaymentAmount { get; set; }

    /// <summary>
    /// Nová výše měsíční splátky
    /// </summary>
    public decimal? NewLoanPaymentAmount { get; set; }

    /// <summary>
    /// V souvislosti s mimořádnou splátkou
    /// </summary>
    public bool ConnectionExtraordinaryPayment { get; set; }
}

/// <summary>
/// Splatnost
/// </summary>
public sealed class DueDateObject
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Aktuální datum splatnosti
    /// </summary>
    public DateTime ActualLoanDueDate { get; set; }

    /// <summary>
    /// Nový datum splatnosti
    /// </summary>
    public DateTime? NewLoanDueDate { get; set; }

    /// <summary>
    /// V souvislosti s mimořádnou splátkou
    /// </summary>
    public bool ConnectionExtraordinaryPayment { get; set; }

}

/// <summary>
/// Účel úvěru
/// </summary>
public sealed class LoanPurposeObject
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Komentář k popisu změny na stávajících účelech úvěru
    /// </summary>
    public DateTime? LoanPurposesComment { get; set; }
}

/// <summary>
/// Podmínky čerpání a další podmínky
/// </summary>
public sealed class DrawingAndOtherConditionsObject
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Komentář ke změně v podmínkách smlouvy
    /// </summary>
    public string? CommentToChangeContractConditions { get; set; }

}

/// <summary>
/// Komentář k žádosti o změnu
/// </summary>
public sealed class CommentToChangeRequestObject
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Obecný komentář
    /// </summary>
    public string? GeneralComment { get; set; }

}