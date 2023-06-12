using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;
using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData.LoanApplicationData;
using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData.ProductRequest;
using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData.ProductRequest.ConditionalValues;
using DomainServices.OfferService.Contracts;
using DomainServices.UserService.Clients;
using DomainServices.UserService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData;

[TransientService, SelfService]
internal class ProductFormData : LoanApplicationBaseFormData
{
    private readonly IUserServiceClient _userService;

    public ProductFormData(HouseholdData householdData, IUserServiceClient userService) : base(householdData)
    {
        _userService = userService;
    }

    public DynamicFormValues MainDynamicFormValues { get; set; } = null!;

    public ConditionalFormValues ConditionalFormValues { get; private set; } = null!;

    public User? PerformerUser { get; private set; }

    public int? SalesArrangementStateId => _codebookManager.SalesArrangementStates.First(x => x.Id == SalesArrangement.State).StarbuildId;

    public int ProductTypeId { get; private set; }

    public decimal? InterestRateDiscount => (decimal?)Offer.SimulationInputs.InterestRateDiscount * -1;

    public int? DrawingTypeId => _codebookManager.DrawingTypes.FirstOrDefault(d => d.Id == Offer.SimulationInputs.DrawingTypeId)?.StarbuildId;

    public int? DrawingDurationId => _codebookManager.DrawingDurations.FirstOrDefault(d => d.Id == Offer.SimulationInputs.DrawingDurationId)?.DrawingDuration;

    public long? MpIdentityId => GetMpIdentityId();

    public bool IsEmployeeBonusRequested => Offer.SimulationInputs.IsEmployeeBonusRequested == true;

    public IEnumerable<ResultFee> OfferFees => Offer.AdditionalSimulationResults.Fees.Where(f => f.UsageText.Contains('F', StringComparison.InvariantCultureIgnoreCase));

    public string? ContractSegment => HouseholdData.Customers.Where(c => c.NaturalPerson.Segment is "PB" or "PC").Select(_ => "PRIV").FirstOrDefault();

    public override Task LoadAdditionalData(CancellationToken cancellationToken)
    {
        ProductTypeId = GetProductTypeId();

        ConditionalFormValues = new ConditionalFormValues(SpecificJsonKeys.Create(ProductTypeId, Offer.SimulationInputs.LoanKindId), this);
        
        return Task.WhenAll(base.LoadAdditionalData(cancellationToken), LoadPerformerData(cancellationToken));
    }

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        base.ConfigureCodebooks(configurator);

        configurator.ProductTypes().DrawingTypes().DrawingDurations().SalesArrangementStates().SalesArrangementTypes();

        HouseholdData.ConfigureCodebooks(configurator);
    }

    private async Task LoadPerformerData(CancellationToken cancellationToken)
    {
        if (MainDynamicFormValues.PerformerUserId is null)
            return;

        PerformerUser = await _userService.GetUser(MainDynamicFormValues.PerformerUserId.Value, cancellationToken);
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