using CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms.FormData.ProductRequest;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms.FormData.LoanApplicationData;

internal abstract class LoanApplicationBaseFormData : AggregatedData
{
    protected LoanApplicationBaseFormData(HouseholdData householdData)
    {
        HouseholdData = householdData;
    }

    public HouseholdData HouseholdData { get; }

    public DefaultValues DefaultValues3601 { get; private set; } = null!;

    public DefaultValues DefaultValues3602 { get; private set; } = null!;

    public IEnumerable<HouseholdDto> HouseholdList => [HouseholdData.HouseholdDto];

    public override Task LoadAdditionalData(CancellationToken cancellationToken)
    {
        DefaultValues3601 = EasFormTypeFactory.CreateDefaultValues(EasFormType.F3601, (SalesArrangementTypes)SalesArrangement.SalesArrangementTypeId, _codebookManager.DocumentTypes);
        DefaultValues3602 = EasFormTypeFactory.CreateDefaultValues(EasFormType.F3602, (SalesArrangementTypes)SalesArrangement.SalesArrangementTypeId,_codebookManager.DocumentTypes);

        HouseholdData.PrepareCodebooks(_codebookManager);

        return HouseholdData.Initialize(SalesArrangement.SalesArrangementId, cancellationToken);
    }

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.DocumentTypes();

        HouseholdData.ConfigureCodebooks(configurator);
    }
}