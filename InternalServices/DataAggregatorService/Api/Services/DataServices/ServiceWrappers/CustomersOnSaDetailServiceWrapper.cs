using CIS.Foms.Enums;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class CustomersOnSaDetailServiceWrapper : IServiceWrapper
{
    private readonly ICustomerOnSAServiceClient _customerOnSaService;

    public CustomersOnSaDetailServiceWrapper(ICustomerOnSAServiceClient customerOnSaService)
    {
        _customerOnSaService = customerOnSaService;
    }

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        if (!input.SalesArrangementId.HasValue)
            throw new ArgumentNullException(nameof(InputParameters.SalesArrangementId));

        var customersOnSa = await _customerOnSaService.GetCustomerList(input.SalesArrangementId.Value, cancellationToken);

        var customersOnSaByRole = customersOnSa.ToLookup(c => (CustomerRoles)c.CustomerRoleId);

        await LoadDebtorDetail(customersOnSaByRole[CustomerRoles.Debtor].FirstOrDefault(), data, cancellationToken);
        await LoadCodebtorDetail(customersOnSaByRole[CustomerRoles.Codebtor].FirstOrDefault(), data, cancellationToken);
    }

    private async Task LoadDebtorDetail(CustomerOnSA? customerOnSa, AggregatedData data, CancellationToken cancellationToken)
    {
        if (customerOnSa is null)
            return;

        data.CustomerOnSaDebtor = await _customerOnSaService.GetCustomer(customerOnSa.CustomerOnSAId, cancellationToken);
    }

    private async Task LoadCodebtorDetail(CustomerOnSA? customerOnSa, AggregatedData data, CancellationToken cancellationToken)
    {
        if (customerOnSa is null)
            return;

        data.CustomerOnSaCodebtor = await _customerOnSaService.GetCustomer(customerOnSa.CustomerOnSAId, cancellationToken);
    }
}