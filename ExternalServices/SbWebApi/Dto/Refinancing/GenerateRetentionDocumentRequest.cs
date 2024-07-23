namespace ExternalServices.SbWebApi.Dto.Refinancing;

public class GenerateRetentionDocumentRequest
{
    public long? CaseId { get; set; }

    public decimal? InterestRate { get; set; }

    public DateTime? DateFrom { get; set; }

    public decimal? PaymentAmount { get; set; }

    /// <summary>
    /// print_signature_form
    /// </summary>
    public int? SignatureTypeDetailId { get; set; }

    public string? Cpm { get; set; }

    public string? Icp { get; set; }

    public DateTime? SignatureDeadline { get; set; }

    public bool? IndividualPricing { get; set; }

    public decimal? Fee { get; set; }
}
