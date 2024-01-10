﻿namespace NOBY.Api.Endpoints.CustomerIncome.SharedDto;

public class IncomeDataEmployement
{
    public int? ForeignIncomeTypeId { get; set; }
    public bool HasProofOfIncome { get; set; }
    public bool HasWageDeduction { get; set; }
    public EmployerDataDto? Employer { get; set; }
    public JobDataDto? Job { get; set; }
    public WageDeductionDataDto? WageDeduction { get; set; }
    public IncomeConfirmationDataDto? IncomeConfirmation { get; set; }
}