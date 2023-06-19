using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.LoanApplication;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;
using DomainServices.CustomerService.Clients;
using DomainServices.HouseholdService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData;

[TransientService, SelfService]
internal class LoanApplication3601TemplateData : LoanApplicationBaseTemplateData
{
    private readonly ICustomerOnSAServiceClient _customerOnSAService;

    protected override HouseholdInfo CurrentHousehold => HouseholdMain!;

    public LoanApplication3601TemplateData(ICustomerServiceClient customerService, ICustomerOnSAServiceClient customerOnSAService, ICustomerChangeDataMerger customerChangeDataMerger) 
        : base(customerService, customerChangeDataMerger)
    {
        _customerOnSAService = customerOnSAService;
    }

    public LoanApplicationCustomer DebtorCustomer => Customer1;

    public LoanApplicationCustomer? CodebtorCustomer => Customer2;

    public LoanApplicationIncome DebtorIncome => Customer1Income;

    public LoanApplicationIncome? CodebtorIncome => Customer2Income;

    public LoanApplicationObligation DebtorObligation => Customer1Obligation;

    public LoanApplicationObligation? CodebtorObligation => Customer2Obligation;

    public string? DebtorMaritalStatus => Customer1MaritalStatus;

    public string? CodebtorMaritalStatus => Customer2MaritalStatus;

    public string LoanType => Offer.SimulationInputs.LoanKindId == 2001 ? GetLoanKindName(Offer.SimulationInputs.LoanKindId) : GetProductTypeName(Offer.SimulationInputs.ProductTypeId);

    public string LoanPurposes => GetLoanPurposes(Offer.SimulationInputs.LoanKindId, Offer.SimulationInputs.LoanPurposes.Select(l => l.LoanPurposeId));

    public decimal? LoanPurposes210 => Offer.SimulationInputs.LoanPurposes.Where(x => x.LoanPurposeId == 210).Select(x => (decimal?)x.Sum).FirstOrDefault();

    public string DrawingTypeName => GetDrawingType();

    public int? DrawingDuration => GetDrawingDuration();

    public string AgentName { get; private set; } = null!;

    public string SignatureType => GetSignatureType();

    public string RealEstateTypes => string.Join("; ", GetRealEstateTypes());

    public string RealEstatePurchaseTypes => string.Join("; ", GetRealEstatePurchaseTypes());

    public override async Task LoadAdditionalData(CancellationToken cancellationToken)
    {
        await base.LoadAdditionalData(cancellationToken);

        AgentName = await LoadAgentName(cancellationToken);
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