using DomainServices.CustomerService.Clients.v1;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class CustomerServiceWrapper : IServiceWrapper
{
    private readonly ICustomerServiceClient _customerService;
    private readonly CustomerWithChangesService _customerWithChangesService;

    public CustomerServiceWrapper(ICustomerServiceClient customerService, CustomerWithChangesService customerWithChangesService)
    {
        _customerService = customerService;
        _customerWithChangesService = customerWithChangesService;
    }

    public DataService DataService => DataService.CustomerService;

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        if (input.CustomerOnSaId.HasValue)
        {
            var (customer, customerOnSA) = await _customerWithChangesService.GetCustomerDetail(input.CustomerOnSaId.Value, cancellationToken);

            data.Customer.Source = customer;
            data.CustomerOnSA = customerOnSA;

            return;
        }

        input.ValidateCustomerIdentities();

        var customerIdentity = input.CustomerIdentities.GetIdentity(Identity.Types.IdentitySchemes.Kb);

        if (input.SalesArrangementId.HasValue)
        {
            var (customer, customerOnSA) = await _customerWithChangesService.GetCustomerDetail(customerIdentity, input.SalesArrangementId.Value, cancellationToken);

            data.Customer.Source = customer;
            data.CustomerOnSA = customerOnSA;
        }
        else
        {
            data.Customer.Source = await _customerService.GetCustomerDetail(customerIdentity, forceKbCustomerLoad: true, cancellationToken);
        }
    }
}