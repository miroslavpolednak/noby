using CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms.FormData.LoanApplicationData;
using CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms.FormData.ProductRequest;
using CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms.FormData.ProductRequest.ConditionalValues;
using DomainServices.OfferService.Contracts;
using DomainServices.UserService.Clients;
using DomainServices.UserService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms.FormData;

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

    public UserInfoObject PerformerUser { get; private set; }

    public bool IsCancelled { get; set; }

    public int? SalesArrangementStateId => IsCancelled ? 2 : _codebookManager.SalesArrangementStates.Where(x => x.Id == SalesArrangement.State).Select(x => x.StarbuildId).FirstOrDefault(1);

    public decimal? InterestRateDiscount => (decimal?)Offer.SimulationInputs.InterestRateDiscount * -1;

    public int? DrawingTypeId => _codebookManager.DrawingTypes.FirstOrDefault(d => d.Id == Offer.SimulationInputs.DrawingTypeId)?.StarbuildId;

    public int? DrawingDurationId => _codebookManager.DrawingDurations.FirstOrDefault(d => d.Id == Offer.SimulationInputs.DrawingDurationId)?.DrawingDuration;

    public long? MpIdentityId => GetMpIdentityId();

    public bool IsEmployeeBonusRequested => Offer.SimulationInputs.IsEmployeeBonusRequested == true;

    public IEnumerable<ResultFee> OfferFees => Offer.AdditionalSimulationResults.Fees.Where(f => f.UsageText.Contains('F', StringComparison.InvariantCultureIgnoreCase));

    public string? ContractSegment => HouseholdData.CustomersHasPRIV ? "PRIV" : default;

    public override Task LoadAdditionalData(InputParameters parameters, CancellationToken cancellationToken)
    {
        ConditionalFormValues = new ConditionalFormValues(SpecificJsonKeys.Create(Case.Data.ProductTypeId, Offer.SimulationInputs.LoanKindId), this);

        return Task.WhenAll(base.LoadAdditionalData(parameters, cancellationToken), LoadPerformerData(cancellationToken));
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

        var user = await _userService.GetUser(MainDynamicFormValues.PerformerUserId.Value, cancellationToken);

        if (string.IsNullOrWhiteSpace(user.UserInfo?.Cpm) || string.IsNullOrWhiteSpace(user.UserInfo?.Icp))
            return;

        PerformerUser = user.UserInfo;
    }

    private long? GetMpIdentityId()
    {
        var customer = HouseholdData.CustomersOnSa.FirstOrDefault(c => c.CustomerOnSAId == SalesArrangement.Mortgage?.Agent);

        return customer?.CustomerIdentifiers?.GetMpIdentityOrDefault()?.IdentityId;
    }
}