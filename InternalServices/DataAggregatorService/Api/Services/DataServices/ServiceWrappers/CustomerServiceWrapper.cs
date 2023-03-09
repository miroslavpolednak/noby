using DomainServices.CustomerService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class CustomerServiceWrapper : IServiceWrapper
{
    private readonly ICustomerServiceClient _customerService;

    public CustomerServiceWrapper(ICustomerServiceClient customerService)
    {
        _customerService = customerService;
    }

    public DataSource DataSource => DataSource.CustomerService;

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateCustomerIdentity();

        data.Customer = await _customerService.GetCustomerDetail(input.CustomerIdentity, cancellationToken);
    }
}