using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.LoanApplication;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData;

[TransientService, SelfService]
internal class LoanApplication3602TemplateData : LoanApplicationBaseTemplateData
{
    private readonly ICustomerServiceClient _customerService;

    public LoanApplication3602TemplateData(ICustomerServiceClient customerService)
    {
        _customerService = customerService;
    }

    public LoanApplicationCustomer? Customer1 { get; private set; }

    public LoanApplicationCustomer? Customer2 { get; private set; }

    public LoanApplicationIncome? Customer1Income { get; private set; }

    public LoanApplicationIncome? Customer2Income { get; private set; }

    public LoanApplicationObligation? Customer1Obligation { get; private set; }

    public LoanApplicationObligation? Customer2Obligation { get; private set; }

    public string? Customer1MaritalStatus => 
        HouseholdMain.Household.Data.AreBothPartnersDeptors == true ? $"{GetMaritalStatus(Customer1)} | druh/družka" : GetMaritalStatus(Customer1);

    public string? Customer2MaritalStatus => 
        HouseholdCodebtor?.Household.Data.AreBothPartnersDeptors == true ? $"{GetMaritalStatus(Customer2)} | druh/družka" : GetMaritalStatus(Customer2);


    public override async Task LoadAdditionalData(CancellationToken cancellationToken)
    {
        var identity1 = HouseholdCodebtor?.CustomerOnSa1?.CustomerIdentifiers.First(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Kb);
        var identity2 = HouseholdCodebtor?.CustomerOnSa2?.CustomerIdentifiers.FirstOrDefault(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Kb);

        var requestedIdentities = new[] { identity1, identity2 }.Where(identity => identity is not null).Cast<Identity>();

        var response = await _customerService.GetCustomerList(requestedIdentities, cancellationToken);

        if (identity1 is not null)
        {
            Customer1 = CreateCustomer(identity1.IdentityId);
            Customer1Income = new LoanApplicationIncome(HouseholdCodebtor!.CustomerOnSa1!);
            Customer1Obligation = new LoanApplicationObligation(HouseholdCodebtor!.CustomerOnSa1!);
        }

        if (identity2 is not null)
        {
            Customer2 = CreateCustomer(identity2.IdentityId);
            Customer2Income = new LoanApplicationIncome(HouseholdCodebtor!.CustomerOnSa2!);
            Customer2Obligation = new LoanApplicationObligation(HouseholdCodebtor!.CustomerOnSa2!);
        }

        LoanApplicationCustomer CreateCustomer(long id) => new(GetDetail(id), _codebookManager.DegreesBefore, _codebookManager.Countries, _codebookManager.IdentificationDocumentTypes, _codebookManager.EducationLevels);
        CustomerDetailResponse GetDetail(long id) => response.Customers.First(c => c.Identities.Any(i => i.IdentityId == id));
    }

    private string? GetMaritalStatus(LoanApplicationCustomer? customer)
    {
        if (customer is null)
            return default;

        return _codebookManager.MaritalStatuses.Where(m => m.Id == customer.MaritalStatusStateId)
                               .Select(m => m.Name)
                               .First();
    }
}