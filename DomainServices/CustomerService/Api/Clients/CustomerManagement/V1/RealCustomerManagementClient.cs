using CIS.Infrastructure.Logging;

namespace DomainServices.CustomerService.Api.Clients.CustomerManagement.V1;

public class RealCustomerManagementClient : BaseClient<ApiException<Error>>, ICustomerManagementClient
{
    public RealCustomerManagementClient(HttpClient httpClient, ILogger<RealCustomerManagementClient> logger) : base(httpClient, logger)
    {
    }

    public Task<CustomerBaseInfo> GetDetail(long customerId, string traceId, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Run inputs: CustomerManagement GetDetail with data {customerId}", customerId);

        return CallEndpoint(GetCustomerDetail);

        async Task<CustomerBaseInfo> GetCustomerDetail()
        {
            var result = await CreateClient().GetCustomerBaseInfoAsync(
                customerId: customerId,
                showPrimaryAddress: true,
                showPrimaryIdDocument: true,
                showCustomerIdentification: true,
                showContactAddress: true,
                showPrimaryPhone: true,
                showPrimaryEmail: true,
                showSegment: null,
                showPoliticallyExposed: true, //TODO overit jestli je potreba pro isPoliticallyExposed nebo jen pro objekt politicalExposition
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
                showBRSubscription: null,
                showTaxResidence: null,
                showCustomerKbRelationship: null,
                showTaxDomicile: null,
                showConfirmedContactFlags: null,
                showBusinessArea: null,
                x_B3_TraceId: traceId,
                x_KB_Caller_System_Identity: "",
                cancellationToken: cancellationToken);

            _logger.LogSerializedObject("CustomerBaseInfo", result);

            return result;
        }
    }

    protected CustomerManagementWrapper CreateClient() => new(_httpClient);

    protected override int GetApiExceptionStatusCode(ApiException<Error> ex) => ex.StatusCode;

    protected override object GetApiExceptionDetail(ApiException<Error> ex) => ex.Result.Detail;
}