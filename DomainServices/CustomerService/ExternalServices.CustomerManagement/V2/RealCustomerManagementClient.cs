using DomainServices.CustomerService.ExternalServices.CustomerManagement.Dto;
using DomainServices.CustomerService.ExternalServices.CustomerManagement.V2.Contracts;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace DomainServices.CustomerService.ExternalServices.CustomerManagement.V2;

internal class RealCustomerManagementClient : ICustomerManagementClient
{
    private readonly HttpClient _httpClient;
    private readonly QueryBuilder _detailQuery;


    public RealCustomerManagementClient(HttpClient httpClient)
    {
        _httpClient = httpClient;

        _detailQuery = CreateDetailQueryBuilder();
    }

    public async Task<CustomerInfo> GetDetail(long customerId, CancellationToken cancellationToken = default)
    {
        var uri = QueryHelpers.AddQueryString($"{_httpClient.BaseAddress}/public/v2/customers/{customerId}/info", _detailQuery!);

        var response = await _httpClient.GetAsync(uri, cancellationToken);

        return await Helpers.ProcessResponse<CustomerInfo>(StartupExtensions.ServiceName, response, cancellationToken);
    }

    public async Task<IReadOnlyList<CustomerInfo>> GetList(IEnumerable<long> customerIds, CancellationToken cancellationToken = default)
    {
        var uri = QueryHelpers.AddQueryString($"{_httpClient.BaseAddress}/public/v2/customers/info", _detailQuery!);

        var response = await _httpClient.PostAsJsonAsync(uri, customerIds, cancellationToken);

        var customers = await Helpers.ProcessResponse<List<CustomerInfo>>(StartupExtensions.ServiceName, response, cancellationToken);

        return customers.AsReadOnly();
    }

    public async Task<IReadOnlyList<CustomerSearchResultRow>> Search(CustomerManagementSearchRequest searchRequest, CancellationToken cancellationToken = default)
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

        if (searchRequest.LegalStatusCode is not null && searchRequest.LegalStatusCode.Any())
            query.Add("legalStatusCode", searchRequest.LegalStatusCode.First().ToString());

        var uri = QueryHelpers.AddQueryString($"{_httpClient.BaseAddress}/public/v2/customers", query);

        var response = await _httpClient.GetAsync(uri, cancellationToken);

        var searchResult = await Helpers.ProcessResponse<CustomerSearchResult>(StartupExtensions.ServiceName, response, cancellationToken);

        return new ReadOnlyCollectionBuilder<CustomerSearchResultRow>(searchResult.ResultRows).ToReadOnlyCollection();
    }

    private static QueryBuilder CreateDetailQueryBuilder()
    {
        return new QueryBuilder
        {
            { "getAllPrimaryPhones", "true" },
            { "requiredAddressFormats", new[] { AddressFormat.COMPONENT, AddressFormat.SINGLE_LINE }.Select(v => v.ToString()) },
            {
                "e", new[]
                {
                    DataEntity.ADDRESS_PRIMARY,
                    DataEntity.ADDRESS_CONTACT,
                    DataEntity.ADDRESS_TEMPORARY_STAY,
                    DataEntity.ID_DOC_PRIMARY,
                    DataEntity.CUSTOMER_IDENTIFICATION,
                    DataEntity.PHONE_PRIMARY,
                    DataEntity.EMAIL_PRIMARY,
                    DataEntity.SEGMENT,
                    DataEntity.POLITICAL_EXPOSITION,
                    DataEntity.BR_SUBSCRIPTION,
                    DataEntity.TAX_RESIDENCE,
                    DataEntity.KB_RELATIONSHIP,
                    DataEntity.EDUCATION,
                    DataEntity.FINANCIAL_PROFILE,
                    DataEntity.EMPLOYMENT
                }.Select(v => v.ToString())
            }
        };
    }
}