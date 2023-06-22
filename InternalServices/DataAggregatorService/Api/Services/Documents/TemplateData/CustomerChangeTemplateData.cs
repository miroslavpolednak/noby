using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.CustomerChange;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData;

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

    public string ReleaseCustomers
    {
        get
        {
            if (SalesArrangement.CustomerChange.Release?.IsActive != true)
                return string.Empty;

            return string.Join(Environment.NewLine,
                               SalesArrangement.CustomerChange.Release.Customers.Select(c => CustomerHelper.NameWithDateOfBirth($"{c.NaturalPerson.FirstName} {c.NaturalPerson.LastName}", c.NaturalPerson.DateOfBirth)));
        }
    }

    public string AddCustomers
    {
        get
        {
            if (SalesArrangement.CustomerChange.Add?.IsActive != true)
                return string.Empty;

            return string.Join(Environment.NewLine, SalesArrangement.CustomerChange.Add.Customers.Select(c => CustomerHelper.NameWithDateOfBirth(c.Name, c.DateOfBirth)));
        }
    }

    public string BankAccount
    {
        get
        {
            if (SalesArrangement.CustomerChange.RepaymentAccount?.IsActive != true)
                return string.Empty;

            var account = SalesArrangement.CustomerChange.RepaymentAccount;

            return BankAccountHelper.AccountNumber(account.Prefix, account.Number, account.BankCode);
        }
    }

    public string BankAccountOwner
    {
        get
        {
            if (SalesArrangement.CustomerChange.RepaymentAccount?.IsActive != true)
                return string.Empty;

            var account = SalesArrangement.CustomerChange.RepaymentAccount;

            return CustomerHelper.NameWithDateOfBirth($"{account.OwnerFirstName} {account.OwnerLastName}", account.OwnerDateOfBirth);
        }
    }

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.Countries().DegreesBefore();
    }

    public override async Task LoadAdditionalData(CancellationToken cancellationToken)
    {
        var customerIdentities = SalesArrangement.CustomerChange.Applicants.Select(a => a.Identity).ToList();

        var customers = await _customerService.GetCustomerList(customerIdentities, SalesArrangement.SalesArrangementId, cancellationToken);

        _customerDetails = customers.Select(customer => new CustomerInfo(customer, _codebookManager.DegreesBefore, _codebookManager.Countries)).ToList();
    }

    private CustomerInfo? GetCustomerInfo(int number) => _customerDetails.ElementAtOrDefault(number - 1);
}