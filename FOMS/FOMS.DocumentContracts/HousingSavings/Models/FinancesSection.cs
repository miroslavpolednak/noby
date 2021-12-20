namespace FOMS.DocumentContracts.HousingSavings.Models;

public class FinancesSection
{
    public int TargetAmount { get; set; }
    public int ActionCode { get; set; }
    public bool IsWoStateSubsidy { get; set; }
    public int? MarketingActionCode { get; set; }
    public bool IsOverpayment { get; set; }
}
