using CIS.Foms.Enums;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class CustomerOnSaServiceWrapper : IServiceWrapper
{
    private readonly ICustomerOnSAServiceClient _customerOnSAService;

    public CustomerOnSaServiceWrapper(ICustomerOnSAServiceClient customerOnSAService)
    {
        _customerOnSAService = customerOnSAService;
    }

    public DataSource DataSource => DataSource.CustomersOnSa;

    public Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task LoadCustomerOnSaDetail(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateSalesArrangementId();

        var customersOnSa = await _customerOnSAService.GetCustomerList(input.SalesArrangementId!.Value, cancellationToken);

        var customersOnSaByRole = customersOnSa.ToLookup(c => (CustomerRoles)c.CustomerRoleId);

        await LoadDebtorDetail(customersOnSaByRole[CustomerRoles.Debtor].FirstOrDefault(), data, cancellationToken);
        await LoadCodebtorDetail(customersOnSaByRole[CustomerRoles.Codebtor].FirstOrDefault(), data, cancellationToken);
    }

    private async Task LoadDebtorDetail(CustomerOnSA? customerOnSa, AggregatedData data, CancellationToken cancellationToken)
    {
        if (customerOnSa is null)
            return;

        data.CustomerOnSaDebtor = await _customerOnSAService.GetCustomer(customerOnSa.CustomerOnSAId, cancellationToken);
    }

    private async Task LoadCodebtorDetail(CustomerOnSA? customerOnSa, AggregatedData data, CancellationToken cancellationToken)
    {
        if (customerOnSa is null)
            return;

        data.CustomerOnSaCodebtor = await _customerOnSAService.GetCustomer(customerOnSa.CustomerOnSAId, cancellationToken);
    }
}