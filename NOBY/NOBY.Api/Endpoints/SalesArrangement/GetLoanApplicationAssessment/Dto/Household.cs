namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment.Dto;

public class Household
{
    /// <summary>
    /// ID domácnosti
    /// </summary>
    public long? HouseholdId { get; set; }

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
