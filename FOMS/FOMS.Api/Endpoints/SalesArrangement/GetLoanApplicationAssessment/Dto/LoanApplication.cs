namespace FOMS.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment.Dto;

public class LoanApplication
{
    public double? LoanApplicationLimit { get; set; }
    public decimal? LoanAmount { get; set; }
    public double? LoanApplicationInstallmentLimit { get; set; }
    public decimal? LoanPaymentAmount { get; set; }
    public double? RemainingAnnuityLivingAmount { get; set; }
    public double? MonthlyIncomeAmount { get; set; }
    public double? MonthlyCostsWithoutInstAmount { get; set; }
    public double? MonthlyInstallmentsInKBAmount { get; set; }
    public double? MonthlyEntrepreneurInstallmentsInKBAmount { get; set; }
    public double? MonthlyInstallmentsInMPSSAmount { get; set; }
    public double? MonthlyInstallmentsInOFIAmount { get; set; }
    public double? MonthlyInstallmentsInCBCBAmount { get; set; }
    public long? DTI { get; set; }
    public long? DSTI { get; set; }
    public long? CIR { get; set; }
    public long? LTV { get; set; }
    public long? LFTV { get; set; }
    public long? LTC { get; set; }
}
