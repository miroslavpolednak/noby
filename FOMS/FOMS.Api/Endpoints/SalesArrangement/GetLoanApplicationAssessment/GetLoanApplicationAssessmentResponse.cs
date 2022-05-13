namespace FOMS.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

public class GetLoanApplicationAssessmentResponse
{
    public int AssessmentResult { get; set; }
    public int LoanApplicationLimit { get; set; }
    public decimal LoanAmount { get; set; }
    public int LoanApplicationInstallmentLimit { get; set; }
    public decimal LoanPaymentAmount { get; set; }
    public int RemainingAnnuityLivingAmount { get; set; }
    public int MonthlyIncomeAmount { get; set; }
    public int MonthlyCostsWithoutInstAmount { get; set; }
    public int MonthlyInstallmentsInKBAmount { get; set; }
    public int MonthlyEntrepreneurInstallmentsInKBAmount { get; set; }
    public int MonthlyInstallmentsInMPSSAmount { get; set; }
    public int MonthlyInstallmentsInOFIAmount { get; set; }
    public int MonthlyInstallmentsInCBCBAmount { get; set; }
    public int DTI { get; set; }
    public int DSTI { get; set; }
    public int CIR { get; set; }
    public int LTV { get; set; }
    public int LFTV { get; set; }
    public int LTC { get; set; }
}
