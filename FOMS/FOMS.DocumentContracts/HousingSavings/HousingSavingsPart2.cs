namespace FOMS.DocumentContracts.HousingSavings;

[ContractPart(typeof(HousingSavingsContract), 2)]
public sealed class HousingSavingsPart2
{
    public Models.FinancesSection? Finances { get; set; }
    public SharedModels.DealerInfo? Dealer { get; set; } 
}
