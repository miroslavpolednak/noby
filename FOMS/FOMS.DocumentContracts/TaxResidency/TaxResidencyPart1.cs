namespace FOMS.DocumentContracts.TaxResidency;

[ContractPart(typeof(TaxResidencyContract), 1)]
public sealed class TaxResidencyPart1
{
    public Models.MainSection? Form { get; set; }
}
