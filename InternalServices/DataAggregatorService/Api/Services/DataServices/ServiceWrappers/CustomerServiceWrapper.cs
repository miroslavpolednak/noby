﻿using DomainServices.CustomerService.Clients;

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

            data.Customer = customer;
            data.CustomerOnSA = customerOnSA;

            return;
        }

        input.ValidateCustomerIdentity();

        if (input.SalesArrangementId.HasValue)
        {
            var (customer, customerOnSA) = await _customerWithChangesService.GetCustomerDetail(input.CustomerIdentity, input.SalesArrangementId.Value, cancellationToken);

            data.Customer = customer;
            data.CustomerOnSA = customerOnSA;
        }
        else
        {
            data.Customer = await _customerService.GetCustomerDetail(input.CustomerIdentity, forceKbCustomerLoad: true, cancellationToken);
        }
    }
}