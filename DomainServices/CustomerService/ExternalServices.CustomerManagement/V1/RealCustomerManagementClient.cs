using CIS.Infrastructure.ExternalServicesHelpers;
using CIS.Infrastructure.Logging;
using DomainServices.CustomerService.ExternalServices.CustomerManagement.Dto;
using DomainServices.CustomerService.ExternalServices.CustomerManagement.V1.Contracts;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Immutable;

namespace DomainServices.CustomerService.ExternalServices.CustomerManagement.V1;

internal sealed class RealCustomerManagementClient 
    : ICustomerManagementClient
{
    public async Task<CustomerBaseInfo> GetDetail(long customerId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var uri = QueryHelpers.AddQueryString(_httpClient.BaseAddress + $"/public/v1/customers/{customerId}/base-info", getUriQuery());
        var response = await _httpClient
            .GetAsync(uri, cancellationToken)
            .ConfigureAwait(false);

        return await processResponse(response, async () =>
            {
                return await response.Content.ReadFromJsonAsync<CustomerBaseInfo>(cancellationToken: cancellationToken)
                    ?? throw new CisExtServiceResponseDeserializationException(0, StartupExtensions.ServiceName, nameof(GetDetail), nameof(CustomerBaseInfo));
            }, cancellationToken);
    }

    public async Task<ImmutableList<CustomerBaseInfo>> GetList(IEnumerable<long> customerIds, CancellationToken cancellationToken = default(CancellationToken))
    {
        var uri = QueryHelpers.AddQueryString(_httpClient.BaseAddress + $"/public/v1/customers/base-info", getUriQuery());
        var response = await _httpClient
            .PostAsJsonAsync(uri, customerIds, cancellationToken)
            .ConfigureAwait(false);

        return (await processResponse(response, async () =>
            {
                return await response.Content.ReadFromJsonAsync<List<CustomerBaseInfo>>(cancellationToken: cancellationToken)
                    ?? throw new CisExtServiceResponseDeserializationException(0, StartupExtensions.ServiceName, nameof(GetList), nameof(List<CustomerBaseInfo>));
            }, cancellationToken))
            .ToImmutableList();
    }

    public async Task<ImmutableList<CustomerSearchResultRow>> Search(CustomerManagementSearchRequest searchRequest, CancellationToken cancellationToken = default(CancellationToken))
    {
        var query = new Dictionary<string, string?>()
        {
            ["numberOfEntries"] = searchRequest.NumberOfEntries,
            ["customerId"] = searchRequest.CustomerId,
            ["name"] = searchRequest.Name,
            [""] = searchRequest,
            [""] = searchRequest,
            [""] = searchRequest,
            [""] = searchRequest,
            [""] = searchRequest,
            [""] = searchRequest,
            [""] = searchRequest,
            [""] = searchRequest,
            [""] = searchRequest,
            [""] = searchRequest,
            [""] = searchRequest,
            [""] = searchRequest,
            [""] = searchRequest,
        };

        var uri = QueryHelpers.AddQueryString(_httpClient.BaseAddress + $"/public/v1/customers", getUriQuery());
        var response = await _httpClient
            .GetAsync(uri, cancellationToken)
            .ConfigureAwait(false);

        return (await processResponse(response, async () =>
        {
            return await response.Content.ReadFromJsonAsync<List<CustomerSearchResultRow>>(cancellationToken: cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, StartupExtensions.ServiceName, nameof(Search), nameof(List<CustomerSearchResultRow>));
        }, cancellationToken))
            .ToImmutableList();





        _logger.LogSerializedObject("Run inputs: CustomerManagement Search", searchRequest);

        return CallEndpoint(SearchCustomers);

        async Task<ICollection<CustomerSearchResultRow>> SearchCustomers()
        {
            var result = await CreateClient().SearchCustomerAsync(
                numberOfEntries: searchRequest.NumberOfEntries,
                customerId: searchRequest.CustomerId,
                name: searchRequest.Name,
                firstName: searchRequest.FirstName,
                birthEstablishedDate: searchRequest.BirthEstablishedDate,
                identifierValue: searchRequest.IdentifierValue,
                identifierTypeCode: searchRequest.IdentifierTypeCode,
                idDocumentTypeCode: searchRequest.IdDocumentTypeCode,
                idDocumentNumber: searchRequest.IdDocumentNumber,
                idDocumentIssuingCountryCode: searchRequest.IdDocumentIssuingCountryCode,
                email: searchRequest.Email,
                phoneNumber: searchRequest.PhoneNumber,
                isInKbi: searchRequest.IsInKbi,
                legalStatusCode: searchRequest.LegalStatusCode,
                includeArchived: searchRequest.IncludeArchived,
                showSegment: searchRequest.ShowSegment,
                showOnlyIdentified: searchRequest.ShowOnlyIdentified,
                x_B3_TraceId: traceId,
                x_KB_Caller_System_Identity: "",
                cancellationToken
            );

            _logger.LogSerializedObject("CustomerSearchResult", result);

            return result.ResultRows;
        }
    }

    private async Task<TResult> processResponse<TResult>(HttpResponseMessage? response, Func<Task<TResult>> successProcessor, CancellationToken cancellationToken)
        where TResult : class
    {
        if (response?.IsSuccessStatusCode ?? false)
        {
            return await successProcessor();
        }
        else if (response?.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var error = await response.Content.ReadFromJsonAsync<Error>(cancellationToken: cancellationToken);
            if (error != null)
                throw new CisExtServiceValidationException($"{error.Message}: {error.Detail}");
        }

        throw new CisExtServiceValidationException($"{StartupExtensions.ServiceName} unknown error {response?.StatusCode}: {await response?.SafeReadAsStringAsync(cancellationToken)}");
    }

    private Dictionary<string, string?> getUriQuery()
        => new Dictionary<string, string?>()
        {
            ["showPrimaryAddress"] = "true",
            ["showPrimaryIdDocument"] = "true",
            ["showCustomerIdentification"] = "true",
            ["showContactAddress"] = "true",
            ["showPrimaryPhone"] = "true",
            ["showPrimaryEmail"] = "true",
            ["showSegment"] = "true",
            ["showPoliticallyExposed"] = "true",
            ["getAllPrimaryPhones"] = "true",
            ["showBRSubscription"] = "true",
            ["showTaxResidence"] = "true",
            ["showCustomerKbRelationship"] = "true",
            ["requiredAddressFormats"] = AddressFormat.LINE.ToString(),
            ["requiredAddressFormats"] = AddressFormat.COMPONENT.ToString()
        };

    private readonly HttpClient _httpClient;
    public RealCustomerManagementClient(HttpClient httpClient)
        => _httpClient = httpClient;
}