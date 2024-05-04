namespace ExternalServices.SbWebApi.Dto.Refinancing;

public class GenerateCalculationDocumentsRequest
{
    public long CaseId { get; set; }

    public bool IsExtraPaymentComplete { get; set; }

    public DateTime ExtraPaymentDate { get; set; }

    public long ClientKbId { get; set; }

    public decimal ExtraPaymentAmount { get; set; }

    public decimal FeeAmount { get; set; }

    public decimal PrincipalAmount { get; set; }

    public decimal InterestAmount { get; set; }

    public decimal OtherUnpaidFees { get; set; }

    public decimal InterestOnLate { get; set; }

    public decimal InterestCovid { get; set; }

    public bool IsLoanOverdue { get; set; }

    public bool IsPaymentReduced { get; set; }

    public DateTime NewMaturityDate { get; set; }

    public decimal NewPaymentAmount { get; set; }

    public int SignatureTypeDetailId { get; set; }
}