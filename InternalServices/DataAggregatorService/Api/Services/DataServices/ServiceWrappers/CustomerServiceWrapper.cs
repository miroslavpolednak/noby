using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomerChangeData;
using DomainServices.CustomerService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class CustomerServiceWrapper : IServiceWrapper
{
    private readonly ICustomerServiceClient _customerService;
    private readonly CustomerChangeDataLoader _customerChangeDataLoader;

    public CustomerServiceWrapper(ICustomerServiceClient customerService, CustomerChangeDataLoader customerChangeDataLoader)
    {
        _customerService = customerService;
        _customerChangeDataLoader = customerChangeDataLoader;
    }

    public DataSource DataSource => DataSource.CustomerService;

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateCustomerIdentity();

        data.Customer = await _customerService.GetCustomerDetail(input.CustomerIdentity, cancellationToken);

        if (input.SalesArrangementId.HasValue) 
            await _customerChangeDataLoader.LoadChangedCustomerData(data.Customer, input.SalesArrangementId.Value, cancellationToken);
    }
}