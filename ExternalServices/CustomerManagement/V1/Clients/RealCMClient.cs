﻿using ExternalServices.CustomerManagement.V1.CMWrapper;
using CIS.Infrastructure.Logging;

namespace ExternalServices.CustomerManagement.V1;

internal sealed class RealCMClient : BaseClient<RealCMClient>, ICMClient
{
    private const string callerSys = "{\"app\":\"DOMAIN_SERVICES\",\"appComp\":\"DOMAIN_SERVICES.CUSTOMER_SERVICE\"}";

    public RealCMClient(HttpClient httpClient, ILogger<RealCMClient> logger) : base(httpClient, logger) { }

    public async Task<IServiceCallResult> GetDetail(long model, string traceId, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Run inputs: CustomerManagement GetDetail with data {model}", model);

        return await WithClient(async c => {

            return await callMethod(async () => {

                var result = await c.GetCustomerBaseInfoAsync(
                    customerId: model, 
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
                    x_B3_TraceId: traceId, 
                    x_KB_Party_Identity_In_Service: "", 
                    x_KB_Orig_System_Identity: callerSys, 
                    x_KB_Caller_System_Identity: callerSys, 
                    cancellationToken: cancellationToken);

                _logger.LogSerializedObject("CustomerBaseInfo", result);

                return new SuccessfulServiceCallResult<CustomerBaseInfo>(result);
            });

        });
    }

    public async Task<IServiceCallResult> GetList(IEnumerable<long> model, string traceId, CancellationToken cancellationToken)
    {
        _logger.LogSerializedObject("Run inputs: CustomerManagement GetList", model);

        return await WithClient(async c => {

            return await callMethod(async () => {

                var result = await c.GetCustomersBaseInfoAsync(
                    body: model, 
                    showPrimaryAddress: true, 
                    showPrimaryIdDocument: true, 
                    showCustomerIdentification: true, 
                    showContactAddress: true, 
                    showPrimaryPhone: true, 
                    showPrimaryEmail: true, 
                    showSegment: null, 
                    showPoliticallyExposed: null,
                    showEsa: null, 
                    showNace: null, 
                    showInsurability: null, 
                    showFirstNameVocative: null, 
                    showSurnameVocative: null, 
                    includeArchived: null, 
                    getAllPrimaryPhones: null, 
                    showFatca: null, 
                    showFinancialProfile: null, 
                    showHousing: null, 
                    showTurnovers: null, 
                    showEducation: null, 
                    showEmployeesNumber: null, 
                    showEmployment: null, 
                    showTemporaryStay: null, 
                    requiredAddressFormats: new List<AddressFormat>() { AddressFormat.LINE, AddressFormat.COMPONENT }, 
                    showBRSubscription: null, 
                    showTaxResidence: null, 
                    showCustomerKbRelationship: null, 
                    x_B3_TraceId: traceId, 
                    x_KB_Party_Identity_In_Service: "", 
                    x_KB_Orig_System_Identity: callerSys, 
                    x_KB_Caller_System_Identity: callerSys, 
                    cancellationToken: cancellationToken);
                _logger.LogSerializedObject("CustomerBaseInfo[]", result);

                return new SuccessfulServiceCallResult<IEnumerable<CustomerBaseInfo>>(result);
            });

        });
    }

    public async Task<IServiceCallResult> Search(SearchCustomerRequest model, string traceId, CancellationToken cancellationToken)
    {
        _logger.LogSerializedObject("Run inputs: CustomerManagement Search", model);

        return await WithClient(async c => {

            return await callMethod(async () => {

                var result = await c.SearchCustomerAsync(model, traceId, callerSys, callerSys, cancellationToken);
                _logger.LogSerializedObject("CustomerSearchResult", result);

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
