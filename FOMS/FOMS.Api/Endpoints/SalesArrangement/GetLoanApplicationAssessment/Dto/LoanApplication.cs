namespace FOMS.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment.Dto;

public class LoanApplication
{
    /// <summary>
    /// Limit výše úvěru
    /// </summary>
    public double? LoanApplicationLimit { get; set; }

    /// <summary>
    /// Požadováno
    /// </summary>
    public decimal? LoanAmount { get; set; }

    /// <summary>
    /// Limit výše splátky
    /// </summary>
    public double? LoanApplicationInstallmentLimit { get; set; }

    /// <summary>
    /// Požadováno
    /// </summary>
    public decimal? LoanPaymentAmount { get; set; }

    /// <summary>
    /// Zbývá na živobytí s požadovanou splátkou
    /// </summary>
    public double? RemainingAnnuityLivingAmount { get; set; }

    /// <summary>
    /// Celkové příjmy
    /// </summary>
    public double? MonthlyIncomeAmount { get; set; }

    /// <summary>
    /// Celkové výdaje (bez splátek)
    /// </summary>
    public double? MonthlyCostsWithoutInstAmount { get; set; }

    /// <summary>
    /// Celkové stávající splátky v KB
    /// </summary>
    public double? MonthlyInstallmentsInKBAmount { get; set; }

    /// <summary>
    /// Celkové stávající splátky FOP v KB
    /// </summary>
    public double? MonthlyEntrepreneurInstallmentsInKBAmount { get; set; }

    /// <summary>
    /// Celkové stávající splátky MPSS
    /// </summary>
    public double? MonthlyInstallmentsInMPSSAmount { get; set; }

    /// <summary>
    /// Celkové prohlášené splátky (klient)
    /// </summary>
    public double? MonthlyInstallmentsInOFIAmount { get; set; }

    /// <summary>
    /// Celkové nalezené splátky (ext. registry)
    /// </summary>
    public double? MonthlyInstallmentsInCBCBAmount { get; set; }

    public long? DTI { get; set; }
    public long? DSTI { get; set; }
    public long? CIR { get; set; }
    public long? LTV { get; set; }
    public long? LFTV { get; set; }
    public long? LTC { get; set; }
}
