using ExternalServices.CustomerManagement.V1.CMWrapper;

namespace ExternalServices.CustomerManagement.V1;

internal sealed class RealCMClient : BaseClient<RealCMClient>, ICMClient
{
    public RealCMClient(HttpClient httpClient, ILogger<RealCMClient> logger) : base(httpClient, logger) { }

    public async Task<IServiceCallResult> GetDetail(long model, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Run inputs: CustomerManagement GetDetail with data {model}", model);

        return await WithClient(async c => {

            return await callMethod(async () => {

                var result = await c.GetCustomerBaseInfoAsync(customerId: model, showPrimaryAddress: true, showPrimaryIdDocument: true, showCustomerIdentification: true, showContactAddress: true, showPrimaryPhone: true, showPrimaryEmail: true, showSegment: null, showPoliticallyExposed: null, showEsa: null, showNace: null, showInsurability: null, showFirstNameVocative: null, showSurnameVocative: null, includeArchived: null, getAllPrimaryPhones: true, showFatca: null, showFinancialProfile: null, showHousing: null, showTurnovers: null, showEducation: null, showEmployeesNumber: null, showEmployment: null, showTemporaryStay: null, requiredAddressFormats: new List<AddressFormat>() { AddressFormat.LINE, AddressFormat.COMPONENT }, showBRSubscription: null, showTaxResidence: null, showCustomerKbRelationship: null, x_B3_TraceId: "", x_KB_Party_Identity_In_Service: "", x_KB_Orig_System_Identity: "", x_KB_Caller_System_Identity: "", cancellationToken: cancellationToken);

                return new SuccessfulServiceCallResult<CustomerBaseInfo>(result);
            });

        });
    }

    public async Task<IServiceCallResult> GetList(IEnumerable<long> model, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Run inputs: CustomerManagement GetList with data {model}", System.Text.Json.JsonSerializer.Serialize(model));

        return await WithClient(async c => {

            return await callMethod(async () => {

                var result = await c.GetCustomersBaseInfoAsync(body: model, showPrimaryAddress: true, showPrimaryIdDocument: true, showCustomerIdentification: true, showContactAddress: true, showPrimaryPhone: true, showPrimaryEmail: true, showSegment: null, showPoliticallyExposed: null, showEsa: null, showNace: null, showInsurability: null, showFirstNameVocative: null, showSurnameVocative: null, includeArchived: null, getAllPrimaryPhones: null, showFatca: null, showFinancialProfile: null, showHousing: null, showTurnovers: null, showEducation: null, showEmployeesNumber: null, showEmployment: null, showTemporaryStay: null, requiredAddressFormats: new List<AddressFormat>() { AddressFormat.LINE, AddressFormat.COMPONENT }, showBRSubscription: null, showTaxResidence: null, showCustomerKbRelationship: null, x_B3_TraceId: "", x_KB_Party_Identity_In_Service: "", x_KB_Orig_System_Identity: "", x_KB_Caller_System_Identity: "", cancellationToken: cancellationToken);

                return new SuccessfulServiceCallResult<IEnumerable<CustomerBaseInfo>>(result);
            });

        });
    }

    public async Task<IServiceCallResult> Search(SearchCustomerRequest model, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Run inputs: CustomerManagement Search with data {model}", System.Text.Json.JsonSerializer.Serialize(model));

        return await WithClient(async c => {

            return await callMethod(async () => {

                var result = await c.SearchCustomerAsync(model, cancellationToken);

                return new SuccessfulServiceCallResult<CustomerSearchResult>(result);
            });

        });
    }

    private Client CreateClient()
        => new(_httpClient?.BaseAddress?.ToString(), _httpClient);

    private async Task<IServiceCallResult> WithClient(Func<Client, Task<IServiceCallResult>> fce)
    {
        try
        {
            return await fce(CreateClient());
        }
        catch (ApiException<Error> ex)
        {
            _logger.LogError(ex, ex.Message);
            return new SuccessfulServiceCallResult<ApiException<Error>>(ex);
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex, ex.Message);
            return new SuccessfulServiceCallResult<ApiException>(ex);
        }
    }
}
