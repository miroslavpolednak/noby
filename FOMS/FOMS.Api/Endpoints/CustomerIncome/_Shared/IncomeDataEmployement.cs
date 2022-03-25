namespace FOMS.Api.Endpoints.CustomerIncome.Dto;

public class IncomeDataEmployement
{
    public bool IsAbroadIncome { get; set; }
    public int? AbroadIncomeTypeId { get; set; }
    public bool IsDomicile { get; set; }
    public EmployerDataDto? Employer { get; set; }
    public JobDataDto? Job { get; set; }
    public WageDeductionDataDto? WageDeduction { get; set; }
    public IncomeConfirmationDataDto? IncomeConfirmation { get; set; }
}
