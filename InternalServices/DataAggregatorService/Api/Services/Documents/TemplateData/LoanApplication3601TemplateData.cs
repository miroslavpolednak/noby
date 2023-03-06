using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.LoanApplication;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData;

[TransientService, SelfService]
internal class LoanApplication3601TemplateData : LoanApplicationBaseTemplateData
{
    private readonly ICustomerServiceClient _customerService;

    public LoanApplication3601TemplateData(ICustomerServiceClient customerService)
    {
        _customerService = customerService;
    }

    public decimal? LoanPurposes210 => Offer.SimulationInputs.LoanPurposes.Where(x => x.LoanPurposeId == 210).Select(x => (decimal?)x.Sum).FirstOrDefault();

    public string DrawingTypeName => GetDrawingType();

    public int? DrawingDuration => GetDrawingDuration();

    public string? DebtorMaritalStatus => HouseholdMain.Household.Data.AreBothPartnersDeptors == true ? $"{GetMaritalStatus(DebtorCustomer)} | druh/družka" : GetMaritalStatus(DebtorCustomer);

    public string? CodebtorMaritalStatus => HouseholdCodebtor?.Household.Data.AreBothPartnersDeptors == true ? $"{GetMaritalStatus(CodebtorCustomer)} | druh/družka" : GetMaritalStatus(CodebtorCustomer);

    public LoanApplicationIncome DebtorIncome => new(CustomerOnSaDebtor);
    
    public LoanApplicationIncome? CodebtorIncome => CustomerOnSaCodebtor is null ? null : new LoanApplicationIncome(CustomerOnSaCodebtor);

    public LoanApplicationObligation DebtorObligation => new(CustomerOnSaDebtor);

    public LoanApplicationObligation? CodebtorObligation => CustomerOnSaCodebtor is null ? null : new LoanApplicationObligation(CustomerOnSaCodebtor);

    public LoanApplicationCustomer DebtorCustomer { get; private set; } = null!;

    public LoanApplicationCustomer? CodebtorCustomer { get; private set; }

    public string HeaderHouseholdData => CustomerOnSaCodebtor is null ? "Údaje o domácnosti žadatele" : "Údaje o společné domácnosti žadatele a spolužadatele";

    public string HeaderHouseholdExpenses => CustomerOnSaCodebtor is null ? "Výdaje domácnosti žadatele (měsíčně)" : "Výdaje domácnosti žadatele a spolužadatele (měsíčně)";

    public override async Task LoadAdditionalData(CancellationToken cancellationToken)
    {
        var debtorIdentity = CustomerOnSaDebtor.CustomerIdentifiers.First(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Kb);
        var codebtorIdentity = CustomerOnSaCodebtor?.CustomerIdentifiers.FirstOrDefault(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Kb);

        var requestedIdentities = new[] { debtorIdentity, codebtorIdentity }.Where(identity => identity is not null).Cast<Identity>();

        var response = await _customerService.GetCustomerList(requestedIdentities, cancellationToken);

        DebtorCustomer = CreateCustomer(debtorIdentity.IdentityId);

        if (codebtorIdentity is not null)
            CodebtorCustomer = CreateCustomer(codebtorIdentity.IdentityId);

        LoanApplicationCustomer CreateCustomer(long id) => new(GetDetail(id), _codebookManager.DegreesBefore, _codebookManager.Countries, _codebookManager.IdentificationDocumentTypes);
        CustomerDetailResponse GetDetail(long id) => response.Customers.First(c => c.Identities.Any(i => i.IdentityId == id));
    }

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        base.ConfigureCodebooks(configurator);

        configurator.DrawingTypes().DrawingDurations();
    }

    private string GetDrawingType() =>
        _codebookManager.DrawingTypes.Where(d => d.Id == Offer.SimulationInputs.DrawingTypeId)
                        .Select(d => d.Name)
                        .DefaultIfEmpty(string.Empty)
                        .First();

    private int? GetDrawingDuration() =>
        _codebookManager.DrawingDurations.Where(d => d.Id == Offer.SimulationInputs.DrawingDurationId)
                        .Select(d => (int?)d.DrawingDuration)
                        .FirstOrDefault();

    private string? GetMaritalStatus(LoanApplicationCustomer? customer)
    {
        if (customer is null)
            return default;

        return _codebookManager.MaritalStatuses.Where(m => m.Id == customer.MaritalStatusStateId)
                               .Select(m => m.Name)
                               .First();
    }
}