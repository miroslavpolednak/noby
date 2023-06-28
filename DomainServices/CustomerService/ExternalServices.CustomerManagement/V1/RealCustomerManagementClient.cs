using DomainServices.CustomerService.ExternalServices.Common;
using DomainServices.CustomerService.ExternalServices.CustomerManagement.Dto;
using DomainServices.CustomerService.ExternalServices.CustomerManagement.V1.Contracts;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using System.Globalization;

namespace DomainServices.CustomerService.ExternalServices.CustomerManagement.V1;

internal sealed class RealCustomerManagementClient 
    : ICustomerManagementClient
{
    public async Task<CustomerBaseInfo> GetDetail(long customerId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var uri = QueryHelpers.AddQueryString(_httpClient.BaseAddress + $"/public/v1/customers/{customerId}/base-info", getQueryBuilder()!);
        var response = await _httpClient
            .GetAsync(uri, cancellationToken)
            .ConfigureAwait(false);

        return await Common.Helpers.ProcessResponse<CustomerBaseInfo>(StartupExtensions.ServiceName, response, cancellationToken);
    }

    public async Task<IReadOnlyList<CustomerBaseInfo>> GetList(IEnumerable<long> customerIds, CancellationToken cancellationToken = default(CancellationToken))
    {
        var uri = QueryHelpers.AddQueryString(_httpClient.BaseAddress + $"/public/v1/customers/base-info", getQueryBuilder()!);
        var response = await _httpClient
            .PostAsJsonAsync(uri, customerIds, cancellationToken)
            .ConfigureAwait(false);

        return (await Common.Helpers.ProcessResponse<List<CustomerBaseInfo>>(StartupExtensions.ServiceName, response, cancellationToken))
            .ToArray().AsReadOnly();
    }

    public async Task<IReadOnlyList<CustomerSearchResultRow>> Search(CustomerManagementSearchRequest searchRequest, CancellationToken cancellationToken = default(CancellationToken))
    {
        var query = new Dictionary<string, string?>
        {
            { "numberOfEntries", searchRequest.NumberOfEntries.ToString() },
            { "customerId", searchRequest.CustomerId.ToString() },
            { "name", searchRequest.Name },
            { "firstName", searchRequest.FirstName },
            { "birthEstablishedDate", searchRequest.BirthEstablishedDate?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) },
            { "identifierValue", searchRequest.IdentifierValue },
            { "identifierTypeCode", searchRequest.IdentifierTypeCode },
            { "idDocumentTypeCode", searchRequest.IdDocumentTypeCode },
            { "idDocumentNumber", searchRequest.IdDocumentNumber },
            { "idDocumentIssuingCountryCode", searchRequest.IdDocumentIssuingCountryCode },
            { "email", searchRequest.Email },
            { "phoneNumber", searchRequest.PhoneNumber }
        };
        if (searchRequest.IsInKbi.HasValue)
            query.Add("isInKbi", searchRequest.IsInKbi.Value.ToQuery());
        if (searchRequest.IncludeArchived.HasValue)
            query.Add("includeArchived", searchRequest.IncludeArchived.Value.ToQuery());
        if (searchRequest.ShowSegment.HasValue)
            query.Add("showSegment", searchRequest.ShowSegment.Value.ToQuery());
        if (searchRequest.ShowOnlyIdentified.HasValue)
            query.Add("showOnlyIdentified", searchRequest.ShowOnlyIdentified.Value.ToQuery());
        if (searchRequest.LegalStatusCode is not null)
            query.Add("legalStatusCode", searchRequest.LegalStatusCode.First().ToString());

        var uri = QueryHelpers.AddQueryString(_httpClient.BaseAddress + $"/public/v1/customers", query);
        var response = await _httpClient
            .GetAsync(uri, cancellationToken)
            .ConfigureAwait(false);

        return (await Common.Helpers.ProcessResponse<CustomerSearchResult>(StartupExtensions.ServiceName, response, cancellationToken))
            .ResultRows.ToArray().AsReadOnly();
    }

    private static QueryBuilder getQueryBuilder()
        => new QueryBuilder
        {
            { "showPrimaryAddress", "true" },
            { "showPrimaryIdDocument", "true" },
            { "showCustomerIdentification", "true" },
            { "showContactAddress", "true" },
            { "showTemporaryStay", "true" },
            { "showPrimaryPhone", "true" },
            { "showPrimaryEmail", "true" },
            { "showSegment", "true" },
            { "showPoliticallyExposed", "true" },
            { "getAllPrimaryPhones", "true" },
            { "showBRSubscription", "true" },
            { "showTaxResidence", "true" },
            { "showCustomerKbRelationship", "true" },
            { "showEducation", "true" },
            { "requiredAddressFormats", new string[] { AddressFormat.LINE.ToString(), AddressFormat.COMPONENT.ToString() } }
        };

    private readonly HttpClient _httpClient;
    public RealCustomerManagementClient(HttpClient httpClient)
        => _httpClient = httpClient;
}