namespace FOMS.DocumentContracts.HousingSavings;

[ContractPart(typeof(HousingSavingsContract), 1)]
public sealed class HousingSavingsPart1
{
    public SharedModels.CustomerDetail? Customer { get; set; }
    public SharedModels.Citizenship? Citizenship { get; set; }
    public SharedModels.SZKU? SZKU { get; set; }
    public TaxResidency.TaxResidencyContract? TaxResidency { get; set; }
}
