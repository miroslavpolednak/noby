using CIS.Core.Results;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class CustomerServiceWrapper : IServiceWrapper
{
    private readonly ICustomerServiceClient _customerService;

    public CustomerServiceWrapper(ICustomerServiceClient customerService)
    {
        _customerService = customerService;
    }

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        if (input.CustomerIdentity is null)
            throw new ArgumentNullException(nameof(InputParameters.CustomerIdentity));

        var result = await _customerService.GetCustomerDetail(input.CustomerIdentity, cancellationToken);

        data.Customer = ServiceCallResult.ResolveAndThrowIfError<CustomerDetailResponse>(result);
    }
}