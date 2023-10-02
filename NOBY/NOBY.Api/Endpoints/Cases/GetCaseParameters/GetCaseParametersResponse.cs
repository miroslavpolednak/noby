﻿using SharedTypes.Types;
using NOBY.Api.Endpoints.Cases.GetCaseParameters.Dto;
using NOBY.Dto;

namespace NOBY.Api.Endpoints.Cases.GetCaseParameters;

public sealed class GetCaseParametersResponse
{
    public DateTime? FirstAnnuityPaymentDate { get; set; }

    /// <summary>
    /// Typ úvěru (číselník).
    /// </summary>
    public DomainServices.CodebookService.Contracts.IBaseCodebook? ProductType { get; set; }

    /// <summary>
	/// ČÍslo smlouvy.
	/// </summary>
    public string? ContractNumber { get; set; }

    /// <summary>
	/// Výše úvěru.
	/// </summary>
    public decimal? LoanAmount { get; set; }

    /// <summary>
	/// Aktuální úroková sazba.
	/// </summary>
    public decimal? LoanInterestRate { get; set; }

    /// <summary>
	/// Datum poskytnutí uvěrové smlouvy.
	/// </summary>
    public DateTime? ContractSignedDate { get; set; }

    /// <summary>
	/// Platnost úrokové sazby do.
	/// </summary>
    public DateTime? FixedRateValidTo { get; set; }

    /// <summary>
	/// Aktuální zůstatek jistiny.
	/// </summary>
    public decimal? Principal { get; set; }

    /// <summary>
	/// Nevyčerpaná částka
	/// </summary>
    public decimal? AvailableForDrawing { get; set; }

    /// <summary>
	/// Datum ukončení čerpání.
	/// </summary>
    public DateTime? DrawingDateTo { get; set; }

    /// <summary>
	/// Výše měsíční splátky.
	/// </summary>
    public decimal? LoanPaymentAmount { get; set; }

    /// <summary>
	/// Druh uveru (číselník).
	/// </summary>
    public DomainServices.CodebookService.Contracts.v1.GenericCodebookResponse.Types.GenericCodebookItem? LoanKind { get; set; }

    /// <summary>
    /// Dlužná částka včetně příslušenství.
    /// </summary>
    public decimal? CurrentAmount { get; set; }

    /// <summary>
	/// Délka fixace úrokové sazby.
	/// </summary>
    public int? FixedRatePeriod { get; set; }

    /// <summary>
	/// Číslo účtu pro splátky.
	/// </summary>
    public BankAccount? PaymentAccount { get; set; }

    /// <summary>
	/// Aktuální částka po splatnosti
	/// </summary>
    public decimal? CurrentOverdueAmount { get; set; }

    /// <summary>
	/// Všechny poplatky po splatnosti.
	/// </summary>
    public decimal? AllOverdueFees { get; set; }

    /// <summary>
    /// Počet dní po splatnosti.
    /// </summary>
    public int? OverdueDaysNumber { get; set; }

    /// <summary>
    /// Účely úvěru
    /// </summary>
    public List<LoanPurposeItem>? LoanPurposes { get; set; }

    /// <summary>
    /// Datum zahájení čerpání.
    /// </summary>
    public DateTime? ExpectedDateOfDrawing { get; set; }

    /// <summary>
    /// Úrok z prodlení.
    /// </summary>
    public decimal? InterestInArrears { get; set; }

    /// <summary>
    /// Datum splatnosti - předpoklad/skutečnost.
    /// </summary>
    public DateTime? LoanDueDate { get; set; }

    /// <summary>
    /// Den splácení.
    /// </summary>
    public int? PaymentDay { get; set; }

    /// <summary>
    /// Nová úroková sazba.
    /// </summary>
    public decimal? LoanInterestRateRefix { get; set; }

    /// <summary>
    /// Platnost nové úrokové sazby od.
    /// </summary>
    public DateTime? LoanInterestRateValidFromRefix { get; set; }

    /// <summary>
    /// Délka nové fixace v měsících.
    /// </summary>
    public int? FixedRatePeriodRefix { get; set; }

    public CaseOwnerUserDto? CaseOwnerOrigUser { get; set; }

    public StatementDto? Statement { get; set; }
}

public sealed class CaseOwnerUserDto
{
    /// <summary>
    /// Pobočka/společnost třetí strany
    /// </summary>
    public string? BranchName { get; set; }

    /// <summary>
    /// Poradce (Jméno Příjmení)
    /// </summary>
    public string? ConsultantName { get; set; }

    /// <summary>
    /// ČPM
    /// </summary>
    public string? Cpm { get; set; }

    /// <summary>
    /// IČP
    /// </summary>
    public string? Icp { get; set; }

    /// <summary>
    /// Všechny identity uživatele z XXVVSS
    /// </summary>
    public List<UserIdentity> UserIdentifiers { get; set; } = null!;
    
    /// <summary>
    /// Flag, zda se jedná o interního uživatele, či externistu
    /// </summary>
    public bool IsInternal { get; set; }
}