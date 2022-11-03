namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment.Dto;

public class LoanApplication
{
    /// <summary>
    /// Limit výše úvěru
    /// </summary>
    public decimal? Limit { get; set; }

    /// <summary>
    /// Požadováno
    /// </summary>
    public decimal? LoanAmount { get; set; }

    /// <summary>
    /// Limit výše splátky
    /// </summary>
    public decimal? InstallmentLimit { get; set; }

    /// <summary>
    /// Požadováno
    /// </summary>
    public decimal? LoanPaymentAmount { get; set; }

    /// <summary>
    /// Zbývá na živobytí s požadovanou splátkou
    /// </summary>
    public decimal? RemainingAnnuityLivingAmount { get; set; }

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
    public decimal? MonthlyInstallmentsInKB { get; set; }

    /// <summary>
    /// Celkové stávající splátky FOP v KB
    /// </summary>
    public decimal? MonthlyEntrepreneurInstallmentsInKB { get; set; }

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
    public decimal? DSTI { get; set; }
    public decimal? CIR { get; set; }
    public decimal? LTV { get; set; }
    public decimal? LFTV { get; set; }
    public decimal? LTCP { get; set; }
}
