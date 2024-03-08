using System.Collections.ObjectModel;
using System.Globalization;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;

namespace ExternalServices.RuianAddress.V1;

internal sealed class RealRuianAddressClient : IRuianAddressClient
{
    private readonly HttpClient _httpClient;

    public RealRuianAddressClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Contracts.AddressDTO> GetAddressDetail(long addressId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/addresses/{addressId}", cancellationToken);

        return await response.EnsureSuccessStatusAndReadJson<Contracts.AddressDTO>(StartupExtensions.ServiceName, cancellationToken);
    }

    public async Task<ICollection<Contracts.AddressDTO>> FindAddresses(string searchText, int pageSize, CancellationToken cancellationToken)
    {
        var queryBuilder = new QueryBuilder
        {
            { "name", searchText },
            { "limit", pageSize.ToString(CultureInfo.InvariantCulture) }
        };

        var response = await _httpClient.GetAsync(QueryHelpers.AddQueryString($"{_httpClient.BaseAddress}/addresses/find", queryBuilder!), cancellationToken);

        var pagedResult = await response.EnsureSuccessStatusAndReadJson<Contracts.AddressDTOPagedResponse>(StartupExtensions.ServiceName, cancellationToken);

        return pagedResult.Items ?? new Collection<Contracts.AddressDTO>();
    }

    public async Task<ICollection<Contracts.TerritoryDTO>> FindTerritory(string searchText, int pageSize, CancellationToken cancellationToken)
    {
        var queryBuilder = new QueryBuilder
        {
            { "name", searchText },
            { "limit", pageSize.ToString(CultureInfo.InvariantCulture) }
        };

        var response = await _httpClient.GetAsync(QueryHelpers.AddQueryString($"{_httpClient.BaseAddress}/territories/find", queryBuilder!), cancellationToken);

        var pagedResult = await response.EnsureSuccessStatusAndReadJson<Contracts.TerritoryDTOPagedResponse>(StartupExtensions.ServiceName, cancellationToken);

        return pagedResult.Items ?? new Collection<Contracts.TerritoryDTO>();
    }
}