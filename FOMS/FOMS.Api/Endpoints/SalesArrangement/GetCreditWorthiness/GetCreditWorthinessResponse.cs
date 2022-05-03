namespace FOMS.Api.Endpoints.SalesArrangement.GetCreditWorthiness;

public sealed class GetCreditWorthinessResponse
{
    public string? WorthinessResult { get; set; }
    public int LoanAmount { get; set; }
    public int MaxAmount { get; set; }
    public int LoanPaymentAmount { get; set; }
    public int InstallmentLimit { get; set; }
    public int RemainsLivingAnnuity { get; set; }
    public int RemainsLivingInst { get; set; }
}
