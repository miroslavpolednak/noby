using CIS.Infrastructure.ExternalServicesHelpers;
using CIS.Infrastructure.Logging;
using DomainServices.CustomerService.ExternalServices.CustomerManagement.Dto;
using DomainServices.CustomerService.ExternalServices.CustomerManagement.V1.Contracts;
using Microsoft.AspNetCore.WebUtilities;

namespace DomainServices.CustomerService.ExternalServices.CustomerManagement.V1;

internal sealed class RealCustomerManagementClient 
    : ICustomerManagementClient
{
    public async Task<CustomerBaseInfo> GetDetail(long customerId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var query = new Dictionary<string, string?>()
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

        var uri = QueryHelpers.AddQueryString(_httpClient.BaseAddress + $"/public/v1/customers/{customerId}/base-info", query);

        var response = await _httpClient
            .GetAsync(uri, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Contracts.WFS_Event_Response>(cancellationToken: cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, StartupExtensions.ServiceName, nameof(CaseStateChanged), nameof(Contracts.WFS_Event_Response));
        }
        else
        {
            throw new CisExtServiceValidationException($"{StartupExtensions.ServiceName} unknown error {response.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}");
        }
    }

    public Task<ICollection<CustomerBaseInfo>> GetList(IEnumerable<long> customerIds, string traceId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.LogSerializedObject("Run inputs: CustomerManagement GetList", customerIds);

        return CallEndpoint(GetCustomerList);

        async Task<ICollection<CustomerBaseInfo>> GetCustomerList()
        {
            var result = await CreateClient().GetCustomersBaseInfoAsync(
                body: customerIds,
                showPrimaryAddress: true,
                showPrimaryIdDocument: true,
                showCustomerIdentification: true,
                showContactAddress: true,
                showPrimaryPhone: true,
                showPrimaryEmail: true,
                showSegment: true,
                showPoliticallyExposed: true,
                showEsa: null,
                showNace: null,
                showInsurability: null,
                showFirstNameVocative: null,
                showSurnameVocative: null,
                includeArchived: null,
                getAllPrimaryPhones: true,
                showFatca: null,
                showFinancialProfile: null,
                showHousing: null,
                showTurnovers: null,
                showEducation: true,
                showEmployeesNumber: null,
                showEmployment: null,
                showTemporaryStay: null,
                requiredAddressFormats: new List<AddressFormat>() { AddressFormat.LINE, AddressFormat.COMPONENT },
                showBRSubscription: true,
                showTaxResidence: true,
                showCustomerKbRelationship: true,
                showTaxDomicile: null,
                showConfirmedContacts: null,
                showBusinessArea: null,
                x_B3_TraceId: traceId,
                x_KB_Caller_System_Identity: "",
                cancellationToken);

            _logger.LogSerializedObject("CustomerBaseInfo[]", result);

            return result;
        }
    }

    public Task<ICollection<CustomerSearchResultRow>> Search(CustomerManagementSearchRequest searchRequest, string traceId, CancellationToken cancellationToken = default(CancellationToken))
    {
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

    private readonly HttpClient _httpClient;
    public RealCustomerManagementClient(HttpClient httpClient)
        => _httpClient = httpClient;
}