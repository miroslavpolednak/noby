using CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.TemplateData.LoanApplication;
using CIS.InternalServices.DataAggregatorService.Api.Services;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;
using DomainServices.HouseholdService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.TemplateData;

[TransientService, SelfService]
internal class LoanApplication3602TemplateData : LoanApplicationBaseTemplateData
{
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    protected override HouseholdInfo CurrentHousehold => HouseholdCodebtor ?? throw CreateNullHouseholdException(nameof(HouseholdCodebtor));

    public LoanApplication3602TemplateData(CustomerWithChangesService customerWithChangesService, ICustomerOnSAServiceClient customerOnSAService) : base(customerWithChangesService)
    {
        _customerOnSAService = customerOnSAService;
    }

    public string LoanDurationText => "Splatnost";

    public string LoanType => Offer.MortgageOffer.SimulationInputs.LoanKindId == 2001 ? GetLoanKindName(Offer.MortgageOffer.SimulationInputs.LoanKindId) : GetProductTypeName(Offer.MortgageOffer.SimulationInputs.ProductTypeId);

    public string LoanPurposes => GetLoanPurposes(Offer.MortgageOffer.SimulationInputs.LoanKindId, Offer.MortgageOffer.SimulationInputs.LoanPurposes.Select(l => l.LoanPurposeId));

    public string AgentName { get; private set; } = null!;

    public string SignatureType => GetSignatureType();

    public override async Task LoadAdditionalData(CancellationToken cancellationToken)
    {
        await base.LoadAdditionalData(cancellationToken);

        AgentName = await LoadAgentName(cancellationToken);
    }

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.SignatureTypes();

        base.ConfigureCodebooks(configurator);
    }

    private async Task<string> LoadAgentName(CancellationToken cancellationToken)
    {
        if (!SalesArrangement.Mortgage.Agent.HasValue)
            return string.Empty;

        var customerOnSa = await _customerOnSAService.GetCustomer(SalesArrangement.Mortgage.Agent.Value, cancellationToken);

        return $"{customerOnSa.FirstNameNaturalPerson} {customerOnSa.Name}";
    }

    private string GetSignatureType()
    {
        if (!SalesArrangement.Mortgage.ContractSignatureTypeId.HasValue)
            return string.Empty;

        return _codebookManager.SignatureTypes.Where(s => s.Id == SalesArrangement.Mortgage.ContractSignatureTypeId.Value)
                               .Select(s => s.Name)
                               .First();
    }
}