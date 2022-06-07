namespace FOMS.Api.Endpoints.CustomerIncome.Dto;

public class JobDataDto
{
    public decimal? GrossAnnualIncome  { get; set; }
    public string? JobDescription { get; set; }
    public bool JobNoticePeriod { get; set; }
    public bool JobTrialPeriod { get; set; }
    public int? EmploymentTypeId { get; set; }
    public DateTime? CurrentWorkContractSince { get; set; }
    public DateTime? CurrentWorkContractTo { get; set; }
    public DateTime? FirstWorkContractSince { get; set; }
}
