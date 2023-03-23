using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;
using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData.ProductRequest;
using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData.ProductRequest.ConditionalValues;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData;

[TransientService, SelfService]
internal class ProductFormData : AggregatedData
{
    public ProductFormData(HouseholdData householdData)
    {
        HouseholdData = householdData;
    }

    public HouseholdData HouseholdData { get; }

    public MockValues MockValues { get; } = new();

    public DefaultValues DefaultValues3601 { get; } = DefaultValuesFactory.Create(EasFormType.F3601);

    public DefaultValues DefaultValues3602 { get; } = DefaultValuesFactory.Create(EasFormType.F3602);

    public ConditionalFormValues ConditionalFormValues { get; private set; } = null!;

    public int? SalesArrangementStateId => _codebookManager.SalesArrangementStates.First(x => x.Id == SalesArrangement.State).StarbuildId;

    public int ProductTypeId { get; private set; }

    public decimal? InterestRateDiscount => (decimal?)Offer.SimulationInputs.InterestRateDiscount * -1;

    public int? DrawingTypeId => _codebookManager.DrawingTypes.FirstOrDefault(d => d.Id == Offer.SimulationInputs.DrawingTypeId)?.StarbuildId;

    public int? DrawingDurationId => _codebookManager.DrawingDurations.FirstOrDefault(d => d.Id == Offer.SimulationInputs.DrawingDurationId)?.DrawingDuration;

    public IEnumerable<HouseholdDto> HouseholdList => new[] { HouseholdData.HouseholdDto };

    public long? MpIdentityId => GetMpIdentityId();

    public bool IsEmployeeBonusRequested => Offer.SimulationInputs.IsEmployeeBonusRequested == true;

    public override Task LoadAdditionalData(CancellationToken cancellationToken)
    {
        ProductTypeId = GetProductTypeId();

        ConditionalFormValues = new ConditionalFormValues(SpecificJsonKeys.Create(ProductTypeId, Offer.SimulationInputs.LoanKindId), this);

        HouseholdData.PrepareCodebooks(_codebookManager);

        return HouseholdData.Initialize(SalesArrangement.SalesArrangementId, cancellationToken);
    }

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.ProductTypes().DrawingTypes().DrawingDurations().SalesArrangementStates().SalesArrangementTypes();

        HouseholdData.ConfigureCodebooks(configurator);
    }

    private int GetProductTypeId()
    {
        var salesArrangementType = _codebookManager.SalesArrangementTypes.FirstOrDefault(t => t.Id == SalesArrangement.SalesArrangementTypeId);

        if (salesArrangementType?.ProductTypeId == null)
            throw new InvalidOperationException(
                $"SalesArrangementType with Id {SalesArrangement.SalesArrangementTypeId} does not exist or ProductTypeId is null.");

        var productType = _codebookManager.ProductTypes.FirstOrDefault(t => t.Id == salesArrangementType.ProductTypeId.Value);

        if (productType is null)
            throw new InvalidOperationException($"ProductType with Id {salesArrangementType.ProductTypeId.Value} does not exist.");

        return productType.Id;
    }

    private long? GetMpIdentityId()
    {
        var customer = HouseholdData.CustomersOnSa.FirstOrDefault(c => c.CustomerOnSAId == SalesArrangement.Mortgage?.Agent);

        return customer?.CustomerIdentifiers
                       .Where(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Mp)
                       .Select(c => (long?)c.IdentityId)
                       .SingleOrDefault();
    }
}