using SharedTypes.GrpcTypes;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Clients.v1;

internal sealed class CustomerServiceClient(Contracts.v1.CustomerService.CustomerServiceClient _service) 
    : ICustomerServiceClient
{
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

    public Task<Customer> GetCustomerDetail(Identity identity, CancellationToken cancellationToken = default)
    {
        return GetCustomerDetail(identity, forceKbCustomerLoad: false, cancellationToken: cancellationToken);
    }

    public Task<Customer> GetCustomerDetail(Identity identity, bool forceKbCustomerLoad, CancellationToken cancellationToken = default)
    {
        return _service.GetCustomerDetailAsync(new GetCustomerDetailRequest { Identity = identity, ForceKbCustomerLoad = forceKbCustomerLoad }, cancellationToken: cancellationToken).ResponseAsync;
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

    public Task<ValidateContactResponse> ValidateContact(ValidateContactRequest request, CancellationToken cancellationToken = default)
    {
        return _service.ValidateContactAsync(request, cancellationToken: cancellationToken).ResponseAsync;
    }

    public async Task<string> FormatAddress(GrpcAddress address, CancellationToken cancellationToken = default)
    {
        var response = await _service.FormatAddressAsync(new FormatAddressRequest { Address = address }, cancellationToken: cancellationToken).ResponseAsync;

        return response.SingleLineAddress;
    }

    public async Task UpdateContacts(Identity identity, IEnumerable<Contact> contacts, CancellationToken cancellationToken = default)
    {
        await _service.UpdateContactsAsync(new UpdateContactsRequest { Identity = identity, Contacts = { contacts } }, cancellationToken: cancellationToken);
    }
}