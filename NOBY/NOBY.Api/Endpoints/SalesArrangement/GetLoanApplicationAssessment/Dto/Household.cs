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
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public CustomerRoles Role { get; set; }

    public List<HouseholdObligationItem>? Existing { get; set; }

    public List<HouseholdObligationItem>? Requested { get; set; }
}

public sealed class HouseholdObligationItem
{
    public string ObligationTypeName { get; set; } = string.Empty;
    public ObligationSource Source { get; set; }
    public string CreditorName { get; set; } = string.Empty;
    public Amount? LoanPrincipalAmount { get; set; }
    public Amount? InstallmentAmount { get; set; }
    public Amount? CreditCardLimit { get; set; }
    public int? CorrectionTypeId { get; set; }

    public enum ObligationSource
    {
        Noby = 1,
        C4M = 2
    }

    public sealed class Amount
    {
        public int Value { get; set; }
        public string Currency { get; set; } = string.Empty;
    }
}