using SharedTypes.Enums;

namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment.Dto;

public class Household
{
    /// <summary>
    /// ID domácnosti
    /// </summary>
    public long? HouseholdId { get; set; }

    public HouseholdRisk Risk { get; set; } = null!;

    public List<HouseholdCustomerObligations>? CustomerObligations { get; set; }
}

public sealed class HouseholdRisk
{
    /// <summary>
    /// Limit výše úvěru
    /// </summary>
    public decimal? LoanApplicationLimit { get; set; }

    /// <summary>
    /// Limit výše splátky
    /// </summary>
    public decimal? LoanApplicationInstallmentLimit { get; set; }

    /// <summary>
    /// Celkové příjmy
    /// </summary>
    public decimal? MonthlyIncome { get; set; }

    /// <summary>
    /// Celkové výdaje (bez splátek)
    /// </summary>
    public decimal? MonthlyCostsWithoutInstallments { get; set; }

    /// <summary>
    /// Celkové stávající splátky v KB
    /// </summary>
    public decimal? MonthlyInstallmentsInKBAmount { get; set; }

    /// <summary>
    /// Celkové stávající splátky FOP v KB
    /// </summary>
    public decimal? MonthlyEntrepreneurInstallmentsInKBAmount { get; set; }

    /// <summary>
    /// Celkové stávající splátky MPSS
    /// </summary>
    public decimal? MonthlyInstallmentsInMPSS { get; set; }

    /// <summary>
    /// Celkové prohlášené splátky (klient)
    /// </summary>
    public decimal? MonthlyInstallmentsInOFI { get; set; }

    /// <summary>
    /// Celkové nalezené splátky (ext. registry)
    /// </summary>
    public decimal? MonthlyInstallmentsInCBCB { get; set; }

    public decimal? DTI { get; set; }
    public long? DSTI { get; set; }
    public long? CIR { get; set; }
}

public sealed class HouseholdCustomerObligations
{
    /// <summary>
    /// Jméno klienta
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Příjmení klienta
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Datum narození klienta
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// Role klienta
    /// </summary>
    public CustomerRoles Role { get; set; }

    /// <summary>
    /// Existující závazky
    /// </summary>
    public List<HouseholdObligationItem>? ExistingObligations { get; set; }

    /// <summary>
    /// Rozpracované závazky
    /// </summary>
    public List<HouseholdObligationItem>? RequestedObligations { get; set; }
}

public sealed class HouseholdObligationItem
{
    public int Id { get; set; }

    /// <summary>
    /// Typ závazku
    /// </summary>
    public int ObligationTypeId { get; set; }

    /// <summary>
    /// Typ závazku - název
    /// </summary>
    public string ObligationTypeName { get; set; } = string.Empty;

    /// <summary>
    /// Stav závazku
    /// </summary>
    public ObligationSource Source { get; set; }

    /// <summary>
    /// Věřitel
    /// </summary>
    public string CreditorName { get; set; } = string.Empty;

    /// <summary>
    /// Nesplacená jistina
    /// </summary>
    public Amount? LoanPrincipalAmount { get; set; }

    /// <summary>
    /// Splátka
    /// </summary>
    public Amount? InstallmentAmount { get; set; }

    /// <summary>
    /// Limit
    /// </summary>
    public Amount? CreditCardLimit { get; set; }

    /// <summary>
    /// Korekce
    /// </summary>
    public int? CorrectionTypeId { get; set; }

    /// <summary>
    /// Kategorie závazku
    /// </summary>
    public int ObligationLaExposureId { get; set; }

    /// <summary>
    /// Kategorie závazku - název
    /// </summary>
    public string ObligationLaExposureName { get; set; }

    /// <summary>
    /// Závazek FOP
    /// </summary>
    public bool ZavazekFOP { get; set; }

    public List<string>? Spoludluznici { get; set; }

    /// <summary>
    /// Kód skupiny
    /// </summary>
    public string? KbGroupInstanceCode { get; set; }

    /// <summary>
    /// Vyčerpaná částka
    /// </summary>
    public int DrawingAmount { get; set; }

    /// <summary>
    /// Číslo  úvěrového účtu
    /// </summary>
    public string? BankAccount { get; set; }

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

    public enum ObligationSource
    {
        Noby = 1,
        C4M = 2
    }

    public sealed class Amount
    {
        /// <summary>
        /// Částka před korekcí
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Částka po korekci
        /// </summary>
        public int? Correction { get; set; }

        /// <summary>
        /// Kód měny
        /// </summary>
        public string CurrencyCode { get; set; } = string.Empty;
    }
}