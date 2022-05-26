namespace FOMS.Api.Endpoints.SalesArrangement.GetCreditWorthiness;

public sealed class GetCreditWorthinessResponse
{
    public int WorthinessResult { get; set; }
    public int MaxAmount { get; set; }
    public int InstallmentLimit { get; set; }
    public int RemainsLivingAnnuity { get; set; }
    public int RemainsLivingInst { get; set; }
    public string? ResultReasonCode { get; set; }
    public string? ResultReasonDescription { get; set; }

    public decimal? LoanAmount { get; set; }
    public decimal? LoanPaymentAmount { get; set; }
    public int DTI { get; set; }
    public int DSTI { get; set; }
}
