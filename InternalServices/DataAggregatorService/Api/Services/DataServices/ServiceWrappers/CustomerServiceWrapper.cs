using DomainServices.CustomerService.Clients;

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

    public DataSource DataSource => DataSource.CustomerService;

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateCustomerIdentity();

        if (input.SalesArrangementId.HasValue)
        {
            data.Customer = await _customerWithChangesService.GetCustomerDetail(input.CustomerIdentity, input.SalesArrangementId.Value, cancellationToken);
        }
        else
        {
            data.Customer = await _customerService.GetCustomerDetail(input.CustomerIdentity, cancellationToken);
        }
    }
}