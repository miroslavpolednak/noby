using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;
using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData.ProductRequest;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData.LoanApplicationData;

internal abstract class LoanApplicationBaseFormData : AggregatedData
{
    protected LoanApplicationBaseFormData(HouseholdData householdData)
    {
        HouseholdData = householdData;
    }

    public HouseholdData HouseholdData { get; }

    public MockValues MockValues { get; } = new();

    public DefaultValues DefaultValues3601 { get; private set; } = null!;

    public DefaultValues DefaultValues3602 { get; private set; } = null!;

    public IEnumerable<HouseholdDto> HouseholdList => new[] { HouseholdData.HouseholdDto };

    public override Task LoadAdditionalData(CancellationToken cancellationToken)
    {
        DefaultValues3601 = EasFormTypeFactory.CreateDefaultValues(EasFormType.F3601, _codebookManager.DocumentTypes);
        DefaultValues3602 = EasFormTypeFactory.CreateDefaultValues(EasFormType.F3602, _codebookManager.DocumentTypes);

        HouseholdData.PrepareCodebooks(_codebookManager);

        return HouseholdData.Initialize(SalesArrangement.SalesArrangementId, cancellationToken);
    }

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.DocumentTypes();

        HouseholdData.ConfigureCodebooks(configurator);
    }
}