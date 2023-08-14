using CIS.Infrastructure.ExternalServicesHelpers;

namespace ExternalServices.RuianAddress.V1;

public interface IRuianAddressClient : IExternalServiceClient
{
    const string Version = "V1";
    const int AllowedCountryId = 16; //Only Czech

    Task<Contracts.AddressDTO> GetAddressDetail(long addressId, CancellationToken cancellationToken);
    Task<ICollection<Contracts.AddressDTO>> FindAddresses(string searchText, int pageSize, CancellationToken cancellationToken);
}