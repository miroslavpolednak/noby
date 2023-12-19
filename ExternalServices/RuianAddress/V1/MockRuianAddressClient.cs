using ExternalServices.RuianAddress.V1.Contracts;

namespace ExternalServices.RuianAddress.V1;

internal class MockRuianAddressClient : IRuianAddressClient
{
    public Task<AddressDTO> GetAddressDetail(long addressId, CancellationToken cancellationToken)
    {
        return Task.FromResult(new AddressDTO());
    }

    public Task<ICollection<AddressDTO>> FindAddresses(string searchText, int pageSize, CancellationToken cancellationToken)
    {
        return Task.FromResult<ICollection<AddressDTO>>(new List<AddressDTO>());
    }

    public Task<ICollection<TerritoryDTO>> FindTerritory(string searchText, int pageSize, CancellationToken cancellationToken)
    {
        return Task.FromResult<ICollection<TerritoryDTO>>(new List<TerritoryDTO>());
    }
}