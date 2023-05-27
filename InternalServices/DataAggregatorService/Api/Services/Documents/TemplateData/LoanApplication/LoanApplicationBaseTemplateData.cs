using CIS.Core.Exceptions;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.LoanApplication;

internal abstract class LoanApplicationBaseTemplateData : AggregatedData
{
    private readonly ICustomerServiceClient _customerService;

    protected abstract HouseholdInfo CurrentHousehold { get; }

    protected LoanApplicationBaseTemplateData(ICustomerServiceClient customerService)
    {
        _customerService = customerService;
    }

    public LoanApplicationCustomer DebtorCustomer { get; private set; } = null!;

    public LoanApplicationCustomer? CodebtorCustomer { get; private set; }

    public LoanApplicationIncome DebtorIncome { get; private set; } = null!;

    public LoanApplicationIncome? CodebtorIncome { get; private set; }

    public LoanApplicationObligation DebtorObligation { get; private set; } = null!;

    public LoanApplicationObligation? CodebtorObligation { get; private set; }

    public string? DebtorMaritalStatus => CurrentHousehold.Household!.Data.AreBothPartnersDeptors == true ? $"{GetMaritalStatus(DebtorCustomer)} | druh/družka" : GetMaritalStatus(DebtorCustomer);

    public string? CodebtorMaritalStatus => CurrentHousehold.Household!.Data.AreBothPartnersDeptors == true ? $"{GetMaritalStatus(CodebtorCustomer)} | druh/družka" : GetMaritalStatus(CodebtorCustomer);

    public string PropertySettlement => GetPropertySettlementName();

    public override Task LoadAdditionalData(CancellationToken cancellationToken)
    {
        var debtorIdentity = CurrentHousehold.Debtor.CustomerIdentifiers.FirstOrDefault(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Kb) ??
                             throw new CisValidationException($"CustomerOnSa (Debtor) {CurrentHousehold.Debtor.CustomerOnSAId} does not have KB identifier.");

        var codebtorIdentity = CurrentHousehold.Codebtor?.CustomerIdentifiers.FirstOrDefault(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Kb);

        return SetDebtorAndCodebtorData(debtorIdentity, codebtorIdentity, cancellationToken);
    }

    private async Task SetDebtorAndCodebtorData(Identity debtor, Identity? codebtor, CancellationToken cancellationToken)
    {
        var response = await _customerService.GetCustomerList(new[] { debtor, codebtor }.Where(identity => identity is not null).Cast<Identity>(), cancellationToken);

        DebtorCustomer = CreateCustomer(debtor.IdentityId);
        DebtorIncome = new LoanApplicationIncome(CurrentHousehold.Debtor);
        DebtorObligation = new LoanApplicationObligation(CurrentHousehold.Debtor);

        if (codebtor is not null)
        {
            CodebtorCustomer = CreateCustomer(codebtor.IdentityId);
            CodebtorIncome = new LoanApplicationIncome(CurrentHousehold.Codebtor!);
            CodebtorObligation = new LoanApplicationObligation(CurrentHousehold.Codebtor!);
        }

        LoanApplicationCustomer CreateCustomer(long id) => new(GetDetail(id), _codebookManager.DegreesBefore, _codebookManager.Countries, _codebookManager.IdentificationDocumentTypes, _codebookManager.EducationLevels);
        CustomerDetailResponse GetDetail(long id) => response.Customers.First(c => c.Identities.Any(i => i.IdentityId == id));
    }

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.Countries().DegreesBefore().LoanKinds().LoanPurposes().ProductTypes()
                    .PropertySettlements().IdentificationDocumentTypes().MaritalStatuses().EducationLevels();
    }

    protected string GetLoanPurposes(int loanKindId, IEnumerable<int> loanPurposeIds)
    {
        if (loanKindId == 2001)
            return "koupě/výstavba/rekonstrukce";

        return string.Join("; ",
                           loanPurposeIds.Select(loanPurposeId => _codebookManager.LoanPurposes.Where(p => p.MandantId == 2 && p.Id == loanPurposeId)
                                                                                  .Select(p => p.Name)
                                                                                  .FirstOrDefault()));
    }

    protected string GetProductTypeName(int productTypeId) =>
        _codebookManager.ProductTypes.Where(x => x.MandantId == 2 && x.Id == productTypeId)
                        .Select(x => x.Name)
                        .DefaultIfEmpty(string.Empty)
                        .First();

    protected string GetLoanKindName(int loanKindId) =>
        _codebookManager.LoanKinds.Where(x => x.MandantId == 2 && x.Id == loanKindId)
                        .Select(x => x.Name)
                        .DefaultIfEmpty(string.Empty)
                        .First();

    private string? GetMaritalStatus(LoanApplicationCustomer? customer)
    {
        if (customer is null)
            return default;

        return _codebookManager.MaritalStatuses.Where(m => m.Id == customer.MaritalStatusStateId)
                               .Select(m => m.Name)
                               .First();
    }

    private string GetPropertySettlementName() =>
        _codebookManager.PropertySettlements.Where(p => p.Id == CurrentHousehold.Household!.Data.PropertySettlementId)
                        .Select(p => p.Name)
                        .DefaultIfEmpty(string.Empty)
                        .First();
}