namespace NOBY.Api.Endpoints.CustomerIncome.Dto;

public class JobDataDto
{
    public decimal? GrossAnnualIncome  { get; set; }
    public string? JobDescription { get; set; }
    public bool IsInProbationaryPeriod { get; set; }
    public bool IsInTrialPeriod { get; set; }
    public int? EmploymentTypeId { get; set; }
    public DateTime? CurrentWorkContractSince { get; set; }
    public DateTime? CurrentWorkContractTo { get; set; }
    public DateTime? FirstWorkContractSince { get; set; }
}
