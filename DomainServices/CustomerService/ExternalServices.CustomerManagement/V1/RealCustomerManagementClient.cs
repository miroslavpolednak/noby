using DomainServices.CustomerService.ExternalServices.Common;
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

        return await Common.Helpers.ProcessResponse<CustomerBaseInfo>(StartupExtensions.ServiceName, response, cancellationToken);
    }

    public async Task<ImmutableList<CustomerBaseInfo>> GetList(IEnumerable<long> customerIds, CancellationToken cancellationToken = default(CancellationToken))
    {
        var uri = QueryHelpers.AddQueryString(_httpClient.BaseAddress + $"/public/v1/customers/base-info", getUriQuery());
        var response = await _httpClient
            .PostAsJsonAsync(uri, customerIds, cancellationToken)
            .ConfigureAwait(false);

        return (await Common.Helpers.ProcessResponse<List<CustomerBaseInfo>>(StartupExtensions.ServiceName, response, cancellationToken))
            .ToImmutableList();
    }

    public async Task<ImmutableList<CustomerSearchResultRow>> Search(CustomerManagementSearchRequest searchRequest, CancellationToken cancellationToken = default(CancellationToken))
    {
        var query = new Dictionary<string, string?>
        {
            { "numberOfEntries", searchRequest.NumberOfEntries.ToString() },
            { "customerId", searchRequest.CustomerId.ToString() },
            { "name", searchRequest.Name },
            { "firstName", searchRequest.FirstName },
            { "birthEstablishedDate", searchRequest.BirthEstablishedDate?.ToString() },
            { "identifierValue", searchRequest.IdentifierValue },
            { "identifierTypeCode", searchRequest.IdentifierTypeCode },
            { "idDocumentTypeCode", searchRequest.IdDocumentTypeCode },
            { "idDocumentNumber", searchRequest.IdDocumentNumber },
            { "idDocumentIssuingCountryCode", searchRequest.IdDocumentIssuingCountryCode },
            { "email", searchRequest.Email },
            { "phoneNumber", searchRequest.PhoneNumber },
            { "isInKbi", searchRequest.IsInKbi.ToQuery() },
            { "includeArchived", searchRequest.IncludeArchived.ToQuery() },
            { "showSegment", searchRequest.ShowSegment.ToQuery() },
            { "showOnlyIdentified", searchRequest.ShowOnlyIdentified.ToQuery() }
        };
        if (searchRequest.LegalStatusCode is not null)
            query.Add("legalStatusCode", searchRequest.LegalStatusCode.First().ToString());

        var uri = QueryHelpers.AddQueryString(_httpClient.BaseAddress + $"/public/v1/customers", query);
        var response = await _httpClient
            .GetAsync(uri, cancellationToken)
            .ConfigureAwait(false);

        return (await Common.Helpers.ProcessResponse<CustomerSearchResult>(StartupExtensions.ServiceName, response, cancellationToken))
            .ResultRows.ToImmutableList();
    }

    private Dictionary<string, string?> getUriQuery()
        => new Dictionary<string, string?>
        {
            { "showPrimaryAddress", "true" },
            { "showPrimaryIdDocument", "true" },
            { "showCustomerIdentification", "true" },
            { "showContactAddress", "true" },
            { "showPrimaryPhone", "true" },
            { "showPrimaryEmail", "true" },
            { "showSegment", "true" },
            { "showPoliticallyExposed", "true" },
            { "getAllPrimaryPhones", "true" },
            { "showBRSubscription", "true" },
            { "showTaxResidence", "true" },
            { "showCustomerKbRelationship", "true" },
            { "requiredAddressFormats", AddressFormat.LINE.ToString() },
            { "requiredAddressFormats", AddressFormat.COMPONENT.ToString() }
        };

    private readonly HttpClient _httpClient;
    public RealCustomerManagementClient(HttpClient httpClient)
        => _httpClient = httpClient;
}