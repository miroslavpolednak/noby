using NOBY.Api.Endpoints.CustomerObligation.SharedDto;
using NOBY.Dto;
using NOBY.Dto.Household;
using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment.V2.Dto;

public sealed class HouseholdObligationItem
{
    [JsonIgnore]
    internal int ObligationTypeOrder;

    /// <summary>
    /// Typ závazku
    /// </summary>
    public int ObligationTypeId { get; set; }

    /// <summary>
    /// Typ závazku - název
    /// </summary>
    public string ObligationTypeName { get; set; } = string.Empty;

    /// <summary>
    /// Stav závazku. 1=NOBY, 2=C4M
    /// </summary>
    public int ObligationSourceId { get; set; }

    /// <summary>
    /// Věřitel
    /// </summary>
    public ObligationCreditorDto? Creditor { get; set; }

    public decimal? AmountConsolidated { get; set; }

    /// <summary>
    /// Nesplacená jistina
    /// </summary>
    public decimal? LoanPrincipalAmount { get; set; }

    /// <summary>
    /// Splátka
    /// </summary>
    public decimal? InstallmentAmount { get; set; }

    /// <summary>
    /// Limit
    /// </summary>
    public decimal? CreditCardLimit { get; set; }

    /// <summary>
    /// Korekce
    /// </summary>
    public CustomerObligationCorrectionDto? Correction { get; set; }

    /// <summary>
    /// Kategorie závazku
    /// </summary>
    public int? ObligationLaExposureId { get; set; }

    /// <summary>
    /// Kategorie závazku - název
    /// </summary>
    public string? ObligationLaExposureName { get; set; }

    /// <summary>
    /// Závazek FOP
    /// </summary>
    public bool IsEntrepreneur { get; set; }

    public List<string>? Codebtors { get; set; }

    /// <summary>
    /// Kód skupiny
    /// </summary>
    public string? KbGroupInstanceCode { get; set; }

    /// <summary>
    /// Vyčerpaná částka
    /// </summary>
    public decimal? DrawingAmount { get; set; }

    /// <summary>
    /// Číslo  úvěrového účtu
    /// </summary>
    public BankAccount? BankAccount { get; set; }

    /// <summary>
    /// Datum poskytnutí
    /// </summary>
    public DateTime? ContractDate { get; set; }

    /// <summary>
    /// Datum splatnosti
    /// </summary>
    public DateTime? MaturityDate { get; set; }

    /// <summary>
    /// Datum aktualizace dat v CBCB
    /// </summary>
    public DateTime? CbcbDataLastUpdate { get; set; }
}