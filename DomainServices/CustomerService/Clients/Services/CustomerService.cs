using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Clients.Services;

internal sealed class CustomerService : ICustomerServiceClient
{
    private readonly Contracts.V1.CustomerService.CustomerServiceClient _service;

    public CustomerService(Contracts.V1.CustomerService.CustomerServiceClient service)
    {
        _service = service;
    }

    public Task<ProfileCheckResponse> ProfileCheck(ProfileCheckRequest request, CancellationToken cancellationToken = default)
    {
        return _service.ProfileCheckAsync(request, cancellationToken: cancellationToken).ResponseAsync;
    }

    public Task<CreateCustomerResponse> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        return _service.CreateCustomerAsync(request, cancellationToken: cancellationToken).ResponseAsync;
    }

    public Task<UpdateCustomerResponse> UpdateCustomer(UpdateCustomerRequest request, CancellationToken cancellationToken= default)
    {
        return _service.UpdateCustomerAsync(request, cancellationToken: cancellationToken).ResponseAsync;
    }

    public Task UpdateCustomerIdentifiers(UpdateCustomerIdentifiersRequest request, CancellationToken cancellationToken = default)
    {
        return _service.UpdateCustomerIdentifiersAsync(request, cancellationToken: cancellationToken).ResponseAsync;
    }

    public Task<CustomerDetailResponse> GetCustomerDetail(Identity identity, CancellationToken cancellationToken = default)
    {
        return _service.GetCustomerDetailAsync(new CustomerDetailRequest { Identity = identity }, cancellationToken: cancellationToken).ResponseAsync;
    }

    public Task<CustomerListResponse> GetCustomerList(IEnumerable<Identity> identities, CancellationToken cancellationToken = default)
    {
        var request = new CustomerListRequest();
        request.Identities.AddRange(identities);

        return _service.GetCustomerListAsync(request, cancellationToken: cancellationToken).ResponseAsync;
    }

    public Task<SearchCustomersResponse> SearchCustomers(SearchCustomersRequest request, CancellationToken cancellationToken = default)
    {
        return _service.SearchCustomersAsync(request, cancellationToken: cancellationToken).ResponseAsync;
    }
}