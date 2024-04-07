namespace ExternalServices.SbWebApi.Dto.Refinancing;

public class GenerateRefixationDocumentRequest
{
    public long CaseId { get; set; }

    public decimal InterestRateProvided { get; set; }

    public DateTime FixedRateValidTo { get; set; }

    public int FixedRatePeriod { get; set; }

    public decimal PaymentAmount { get; set; }

    public int SignatureTypeDetailId { get; set; }

    public string Cpm { get; set; } = null!;

    public string Icp { get; set; } = null!;

    public DateTime? SignatureDeadline { get; set; }

    public bool IndividualPricing { get; set; }
}