namespace FOMS.DocumentContracts.HousingSavings;

[ContractPart(typeof(HousingSavingsContract), 1)]
public sealed class HousingSavingsPart1
{
    public SharedModels.CustomerDetail? Customer { get; set; }
    public SharedModels.Citizenship? Citizenship { get; set; }
}
