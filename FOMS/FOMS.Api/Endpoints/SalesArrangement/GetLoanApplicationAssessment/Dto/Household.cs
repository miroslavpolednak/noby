namespace FOMS.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment.Dto;

public class Household
{
    public int HouseholdId { get; set; }
    public int LoanApplicationLimit { get; set; }
    public int LoanApplicationInstallmentLimit { get; set; }
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
}
