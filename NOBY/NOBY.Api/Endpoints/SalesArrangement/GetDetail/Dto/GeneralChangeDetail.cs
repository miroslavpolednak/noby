﻿using CIS.Foms.Types;
using NOBY.Api.Endpoints.SalesArrangement.Dto;
using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.SalesArrangement.GetDetail.Dto;

public sealed class GeneralChangeDetail
{
    /// <summary>
    /// Identita klienta
    /// </summary>
    public CustomerIdentity? Applicant { get; set; }

    /// <summary>
    /// Zajištění
    /// </summary>
    [Required]
    public CollateralObject Collateral { get; set; }

    /// <summary>
    /// Den splácení
    /// </summary>
    [Required]
    public PaymentDayObject PaymentDay { get; set; }

    /// <summary>
    /// Lhůta ukončení čerpání
    /// </summary>
    [Required]
    public DrawingDateToObject DrawingDateTo { get; set; }

    /// <summary>
    /// Účet pro splácení
    /// </summary>
    [Required]
    public PaymentAccountObject RepaymentAccount { get; set; }

    /// <summary>
    /// Výše měsíční splátky
    /// </summary>
    [Required]
    public LoanPaymentAmountObject LoanPaymentAmount { get; set; }

    /// <summary>
    /// Splatnost
    /// </summary>
    [Required]
    public DueDateObject DueDate { get; set; }

    /// <summary>
    /// Objekty úvěru
    /// </summary>
    [Required]
    public LoanRealEstateObject LoanRealEstate { get; set; }

    /// <summary>
    /// Účel úvěru
    /// </summary>
    [Required]
    public LoanPurposeObject LoanPurpose { get; set; }

    /// <summary>
    /// Podmínky čerpání a další podmínky
    /// </summary>
    [Required]
    public DrawingAndOtherConditionsObject DrawingAndOtherConditions { get; set; }

    /// <summary>
    /// Komentář k žádosti o změnu
    /// </summary>
    [Required]
    public CommentToChangeRequestObject CommentToChangeRequest { get; set; }
}

/// <summary>
/// Den splácení
/// </summary>
public sealed class PaymentDayObject
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    [Required]
    public bool IsActive { get; set; }

    /// <summary>
    /// Sjednaný den splácení
    /// </summary>
    [Required]
    public int? AgreedPaymentDay { get; set; }

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
    [Required]
    public bool IsActive { get; set; }

    /// <summary>
    /// Sjednaný termín čerpání do
    /// </summary>
    [Required]
    public DateTime? AgreedDrawingDateTo { get; set; }

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
public sealed class PaymentAccountObject
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    [Required]
    public bool IsActive { get; set; }

    /// <summary>
    /// Předčíslí účtu
    /// </summary>
    [Required]
    public string? AgreedPrefix { get; set; }

    /// <summary>
    /// Číslo účtu
    /// </summary>
    [Required]
    public string? AgreedNumber { get; set; }

    /// <summary>
    /// Kód banky
    /// </summary>
    [Required]
    public string? AgreedBankCode { get; set; }

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
    [Required]
    public bool IsActive { get; set; }

    /// <summary>
    /// Aktuální výše měsíční splátky
    /// </summary>
    [Required]
    public decimal? ActualLoanPaymentAmount { get; set; }

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
public sealed class DueDateObject
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Aktuální datum splatnosti
    /// </summary>
    [Required]
    public DateTime? ActualLoanDueDate { get; set; }

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