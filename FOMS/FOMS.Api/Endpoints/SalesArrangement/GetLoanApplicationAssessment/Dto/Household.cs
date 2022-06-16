namespace FOMS.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment.Dto;

public class Household
{
    public long? HouseholdId { get; set; }
    public double? LoanApplicationLimit { get; set; }
    public double? LoanApplicationInstallmentLimit { get; set; }
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
}
