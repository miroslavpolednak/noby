namespace NOBY.Dto.Refinancing;
public class ProcessDetail
{
    public long ProcessId { get; set; }

    public int RefinancingTypeId { get; set; }

    public string? RefinancingTypeText { get; set; }

    public int RefinancingStateId { get; set; }

    public DateTime? CreatedTime { get; set; }

    public string? CreatedBy { get; set; }

    public decimal? LoanInterestRateProvided { get; set; }

    public string? LoanInterestRateValidFrom { get; set; }

    public string? LoanInterestRateValidTo { get; set; }

    public DateTime? EffectiveDate { get; set; }

    public string? DocumentId { get; set; }
}
