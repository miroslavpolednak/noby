using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData.ProductRequest;
using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData.ProductRequest.ConditionalValues;
using DomainServices.CodebookService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData;

internal class ProductFormData : AggregatedData
{
    private List<DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementStates.SalesArrangementStateItem> _salesArrangementStates = null!;
    private List<DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementTypes.SalesArrangementTypeItem> _salesArrangementTypes = null!;
    private List<DomainServices.CodebookService.Contracts.Endpoints.ProductTypes.ProductTypeItem> _productTypes = null!;
    private List<DomainServices.CodebookService.Contracts.Endpoints.DrawingTypes.DrawingTypeItem> _drawingTypes = null!;
    private List<DomainServices.CodebookService.Contracts.Endpoints.DrawingDurations.DrawingDurationItem> _drawingDurations = null!;

    public ProductFormData(HouseholdData householdData)
    {
        HouseholdData = householdData;
    }

    public AggregatedData AggregatedData => this;

    public HouseholdData HouseholdData { get; }

    public MockValues MockValues { get; } = new();

    public DefaultValues DefaultValues3601 { get; } = DefaultValuesFactory.Create(EasFormType.F3601);

    public DefaultValues DefaultValues3602 { get; } = DefaultValuesFactory.Create(EasFormType.F3602);

    public DynamicFormValues? DynamicFormValues { get; set; }

    public ConditionalFormValues ConditionalFormValues { get; private set; } = null!;

    public int? SalesArrangementStateId => _salesArrangementStates.First(x => x.Id == SalesArrangement.State).StarbuildId;

    public DateTime FirstSignedDate => (DateTime?)SalesArrangement.FirstSignedDate ?? DateTime.Now;

    public int ProductTypeId { get; private set; }

    public decimal? InterestRateDiscount => (decimal?)Offer.SimulationInputs.InterestRateDiscount * 1;

    public int? DrawingTypeId => _drawingTypes.FirstOrDefault(d => d.Id == Offer.SimulationInputs.DrawingTypeId)?.StarbuildId;

    public int? DrawingDurationId => _drawingDurations.FirstOrDefault(d => d.Id == Offer.SimulationInputs.DrawingDurationId)?.DrawingDuration;

    public IEnumerable<HouseholdDto> HouseholdList => new[] { HouseholdData.HouseholdDto };

    public long? MpIdentityId => GetMpIdentityId();

    public bool IsEmployeeBonusRequested => Offer.SimulationInputs.IsEmployeeBonusRequested == true;

    public override Task LoadAdditionalData(CancellationToken cancellationToken)
    {
        ProductTypeId = GetProductTypeId();

        ConditionalFormValues = new ConditionalFormValues(SpecificJsonKeys.Create(ProductTypeId, Offer.SimulationInputs.LoanKindId), this);

        return HouseholdData.Initialize(SalesArrangement.SalesArrangementId);
    }

    public override async Task LoadCodebooks(ICodebookServiceClients codebookService, CancellationToken cancellationToken)
    {
        _salesArrangementStates = await codebookService.SalesArrangementStates(cancellationToken);
        _salesArrangementTypes = await codebookService.SalesArrangementTypes(cancellationToken);
        _productTypes = await codebookService.ProductTypes(cancellationToken);
        _drawingTypes = await codebookService.DrawingTypes(cancellationToken);
        _drawingDurations = await codebookService.DrawingDurations(cancellationToken);

        await HouseholdData.LoadCodebooks(codebookService);
    }

    private int GetProductTypeId()
    {
        var salesArrangementType = _salesArrangementTypes.FirstOrDefault(t => t.Id == SalesArrangement.SalesArrangementTypeId);

        if (salesArrangementType?.ProductTypeId == null)
            throw new InvalidOperationException(
                $"SalesArrangementType with Id {SalesArrangement.SalesArrangementTypeId} does not exist or ProductTypeId is null.");

        var productType = _productTypes.FirstOrDefault(t => t.Id == salesArrangementType.ProductTypeId.Value);

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