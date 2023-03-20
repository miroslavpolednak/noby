using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.CustomerChange;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData;

internal class CustomerChangeTemplateData : AggregatedData
{
    private IList< CustomerDetailResponse> _customerDetails = null!;

    public CustomerChangeTemplateData()
    {
    }

    public CustomerInfo Customer1 => GetCustomerInfo(1)!;

    public CustomerInfo? Customer2 => GetCustomerInfo(2);

    public CustomerInfo? Customer3 => GetCustomerInfo(3);

    public CustomerInfo? Customer4 => GetCustomerInfo(4);

    public string ReleaseCustomers
    {
        get
        {
            if (!SalesArrangement.CustomerChange.Release.IsActive)
                return string.Empty;

            return string.Join(Environment.NewLine, SalesArrangement.CustomerChange.Release.Customers.Select(c => $"{c.NaturalPerson.FirstName} {c.NaturalPerson.LastName}, datum narození: {c.NaturalPerson.DateOfBirth}"));
        }
    }

    public string AddCustomers
    {
        get
        {
            if (!SalesArrangement.CustomerChange.Add.IsActive)
                return string.Empty;

            return string.Join(Environment.NewLine, SalesArrangement.CustomerChange.Add.Customers.Select(c => $"{c.Name}, datum narození: {c.DateOfBirth}"));
        }
    }

    public string BankAccount
    {
        get
        {
            if (!SalesArrangement.CustomerChange.RepaymentAccount.IsActive)
                return string.Empty;

            if (string.IsNullOrWhiteSpace(SalesArrangement.CustomerChange.RepaymentAccount.Prefix))
                return $"{SalesArrangement.CustomerChange.RepaymentAccount.Number}/{SalesArrangement.CustomerChange.RepaymentAccount.BankCode}";

            return $"{SalesArrangement.CustomerChange.RepaymentAccount.Prefix}-{SalesArrangement.CustomerChange.RepaymentAccount.Number}/{SalesArrangement.CustomerChange.RepaymentAccount.BankCode}";
        }
    }

    public string BankAccountOwner
    {
        get
        {
            if (!SalesArrangement.CustomerChange.RepaymentAccount.IsActive)
                return string.Empty;

            return $"{SalesArrangement.CustomerChange.RepaymentAccount.OwnerFirstName} {SalesArrangement.CustomerChange.RepaymentAccount.OwnerLastName}, " +
                   $"datum narození: {SalesArrangement.CustomerChange.RepaymentAccount.OwnerDateOfBirth}";
        }
    }

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.Countries().DegreesBefore();
    }

    public override async Task LoadAdditionalData(CancellationToken cancellationToken)
    {
        //var customerIdentities = SalesArrangement.CustomerChange.Applicants.Select(a => a.Identity).ToList();

        //_customerDetails = (await _customerService.GetCustomerList(customerIdentities, cancellationToken)).Customers;
    }

    private CustomerInfo? GetCustomerInfo(int number) => 
        _customerDetails.Select(customer => new CustomerInfo(customer, _codebookManager.DegreesBefore, _codebookManager.Countries)).ElementAtOrDefault(number - 1);
}