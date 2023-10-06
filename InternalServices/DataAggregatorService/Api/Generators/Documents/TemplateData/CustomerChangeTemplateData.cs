using CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.TemplateData.CustomerChange;
using CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.TemplateData.Shared;
using CIS.InternalServices.DataAggregatorService.Api.Services;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.TemplateData;

[TransientService, SelfService]
internal class CustomerChangeTemplateData : AggregatedData
{
    private readonly CustomerWithChangesService _customerService;

    private IList<CustomerInfo> _customerDetails = null!;

    public CustomerChangeTemplateData(CustomerWithChangesService customerService)
    {
        _customerService = customerService;
    }

    public CustomerInfo Customer1 => GetCustomerInfo(1)!;

    public CustomerInfo? Customer2 => GetCustomerInfo(2);

    public CustomerInfo? Customer3 => GetCustomerInfo(3);

    public CustomerInfo? Customer4 => GetCustomerInfo(4);

    public string PaymentAccount => BankAccountHelper.AccountNumber(Mortgage.PaymentAccount.Prefix, Mortgage.PaymentAccount.Number, Mortgage.PaymentAccount.BankCode);

    public string ReleaseCustomers
    {
        get
        {
            var customersText = Enumerable.Empty<string>();

            if (SalesArrangement.CustomerChange.Release?.IsActive == true)
                customersText = SalesArrangement.CustomerChange.Release.Customers.Select(c => CustomerHelper.NameWithDateOfBirth($"{c.NaturalPerson.FirstName} {c.NaturalPerson.LastName}", c.NaturalPerson.DateOfBirth));

            return string.Join(Environment.NewLine, customersText.Concat(Enumerable.Repeat("--", 4)).Take(4));
        }
    }

    public string AddCustomers
    {
        get
        {
            var customersText = Enumerable.Empty<string>();

            if (SalesArrangement.CustomerChange.Add?.IsActive == true)
                customersText = SalesArrangement.CustomerChange.Add.Customers.Select(c => CustomerHelper.NameWithDateOfBirth(c.Name, c.DateOfBirth));

            return string.Join(Environment.NewLine, customersText.Concat(Enumerable.Repeat("--", 4)).Take(4));
        }
    }

    public string? BankAccount
    {
        get
        {
            if (SalesArrangement.CustomerChange.RepaymentAccount?.IsActive != true)
                return default;

            var account = SalesArrangement.CustomerChange.RepaymentAccount;

            return BankAccountHelper.AccountNumber(account.Prefix, account.Number, account.BankCode);
        }
    }

    public string? BankAccountOwner
    {
        get
        {
            if (SalesArrangement.CustomerChange.RepaymentAccount?.IsActive != true)
                return null;

            var account = SalesArrangement.CustomerChange.RepaymentAccount;

            return CustomerHelper.NameWithDateOfBirth($"{account.OwnerFirstName} {account.OwnerLastName}", account.OwnerDateOfBirth);
        }
    }

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.DegreesBefore();
    }

    public override async Task LoadAdditionalData(CancellationToken cancellationToken)
    {
        var customerIdentities = SalesArrangement.CustomerChange.Applicants.Select(a => a.Identity.GetIdentity(Identity.Types.IdentitySchemes.Kb));

        var customers = await _customerService.GetCustomerList(customerIdentities, SalesArrangement.SalesArrangementId, cancellationToken);

        _customerDetails = customers.Select(customer => new CustomerInfo(customer, _codebookManager.DegreesBefore)).ToList();
    }

    private CustomerInfo? GetCustomerInfo(int number) => _customerDetails.ElementAtOrDefault(number - 1);
}