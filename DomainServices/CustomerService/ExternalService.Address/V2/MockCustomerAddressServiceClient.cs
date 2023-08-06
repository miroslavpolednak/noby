using DomainServices.CustomerService.ExternalServices.Address.V2.Contracts;

namespace DomainServices.CustomerService.ExternalServices.Address.V2;

internal class MockCustomerAddressServiceClient : ICustomerAddressServiceClient
{
    public Task<string> FormatAddress(ComponentAddressPoint componentAddress, CancellationToken cancellationToken)
    {
        return Task.FromResult("Pražská 125, PRAHA, PSČ 111 11, Česká republika");
    }
}