﻿using DomainServices.CustomerService.Clients;

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

        data.Customer = await _customerService.GetCustomerDetail(input.CustomerIdentity, cancellationToken);
    }
}