using System.Net.Http.Json;
using DomainServices.CustomerService.ExternalServices.Address.V2.Contracts;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;

namespace DomainServices.CustomerService.ExternalServices.Address.V2;

internal class RealCustomerAddressServiceClient : ICustomerAddressServiceClient
{
    private readonly HttpClient _httpClient;

    public RealCustomerAddressServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> FormatAddress(ComponentAddressPoint componentAddress, CancellationToken cancellationToken)
    {
        var request = new AddressToFormat { ComponentAddressPoint = componentAddress };

        var queryBuilder = new QueryBuilder
        {
            { "requiredAddressFormats", new[] { AddressFormat.SINGLE_LINE.ToString() } }
        };

        var response = await _httpClient.PostAsJsonAsync(QueryHelpers.AddQueryString($"{_httpClient.BaseAddress}/public/v2/address/format-address", queryBuilder!), request, cancellationToken);

        if (!response.IsSuccessStatusCode) 
            return string.Empty;

        var formattedAddress = await response.Content.ReadFromJsonAsync<FormattedAddress>(cancellationToken: cancellationToken);

        return formattedAddress!.SingleLineAddressPoint.Address;

    }
}