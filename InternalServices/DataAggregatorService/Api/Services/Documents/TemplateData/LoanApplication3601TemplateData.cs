using CIS.Core.Exceptions;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.LoanApplication;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData;

[TransientService, SelfService]
internal class LoanApplication3601TemplateData : LoanApplicationBaseTemplateData
{
    private readonly ICustomerServiceClient _customerService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;

    public LoanApplication3601TemplateData(ICustomerServiceClient customerService, ICustomerOnSAServiceClient customerOnSAService)
    {
        _customerService = customerService;
        _customerOnSAService = customerOnSAService;
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

    public string AgentName { get; private set; } = null!;

    public string SignatureType => GetSignatureType();

    public string RealEstateTypes => string.Join("; ", GetRealEstateTypes());

    public string RealEstatePurchaseTypes => string.Join("; ", GetRealEstatePurchaseTypes());

    public override async Task LoadAdditionalData(CancellationToken cancellationToken)
    {
        var debtorIdentity = CustomerOnSaDebtor.CustomerIdentifiers.FirstOrDefault(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Kb);
        if (debtorIdentity is null)
            throw new CisValidationException($"CustomerOnSa {CustomerOnSaDebtor.CustomerOnSAId} does not have KB identifier.");

        var codebtorIdentity = CustomerOnSaCodebtor?.CustomerIdentifiers.FirstOrDefault(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Kb);

        var requestedIdentities = new[] { debtorIdentity, codebtorIdentity }.Where(identity => identity is not null).Cast<Identity>();

        var response = await _customerService.GetCustomerList(requestedIdentities, cancellationToken);
        
        DebtorCustomer = CreateCustomer(debtorIdentity.IdentityId);

        if (codebtorIdentity is not null)
            CodebtorCustomer = CreateCustomer(codebtorIdentity.IdentityId);

        AgentName = await LoadAgentName(cancellationToken);

        LoanApplicationCustomer CreateCustomer(long id) => new(GetDetail(id), _codebookManager.DegreesBefore, _codebookManager.Countries, _codebookManager.IdentificationDocumentTypes, _codebookManager.EducationLevels);
        CustomerDetailResponse GetDetail(long id) => response.Customers.First(c => c.Identities.Any(i => i.IdentityId == id));
    }

    private async Task<string> LoadAgentName(CancellationToken cancellationToken)
    {
        if (!SalesArrangement.Mortgage.Agent.HasValue)
            return string.Empty;

        var customerOnSa = await _customerOnSAService.GetCustomer(SalesArrangement.Mortgage.Agent.Value, cancellationToken);

        return $"{customerOnSa.FirstNameNaturalPerson} {customerOnSa.Name}";
    }

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        base.ConfigureCodebooks(configurator);

        configurator.DrawingTypes().DrawingDurations().SignatureTypes().RealEstateTypes().PurchaseTypes();
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

    private string GetSignatureType()
    {
        if (!SalesArrangement.Mortgage.ContractSignatureTypeId.HasValue)
            return string.Empty;

        return _codebookManager.SignatureTypes.Where(s => s.Id == SalesArrangement.Mortgage.ContractSignatureTypeId.Value)
                               .Select(s => s.Name)
                               .First();
    }

    private IEnumerable<string> GetRealEstateTypes() =>
        SalesArrangement.Mortgage
                        .LoanRealEstates
                        .Join(_codebookManager.RealEstateTypes, x => x.RealEstateTypeId, y => y.Id, (_, y) => y.Name);

    private IEnumerable<string> GetRealEstatePurchaseTypes() =>
        SalesArrangement.Mortgage
                        .LoanRealEstates
                        .Join(_codebookManager.PurchaseTypes, x => x.RealEstatePurchaseTypeId, y => y.Id, (_, y) => y.Name);
}