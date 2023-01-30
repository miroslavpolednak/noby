using CIS.Infrastructure.ExternalServicesHelpers;
using ExternalServices.SbWebApi.Dto;

namespace ExternalServices.SbWebApi.V1;

internal sealed class RealSbWebApiClient
    : ISbWebApiClient
{
    public async Task CaseStateChanged(CaseStateChangedRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        // vytvoreni EAS requestu
        var easRequest = new Contracts.WFS_Request_CaseStateChanged
        {
            Header = new Contracts.WFS_Header
            {
                System = "NOBY",
                Login = request.Login
            },
            Message = new Contracts.WFS_Event_CaseStateChanged
            {
                Client_benefits = 0,
                Case_id = request.CaseId,
                Uver_id = request.CaseId,
                Loan_no = request.ContractNumber,
                Jmeno_prijmeni = request.ClientFullName,
                Case_state = request.CaseStateName,
                Product_type = request.ProductTypeId,
                Owner_cpm = request.OwnerUserCpm,
                Owner_icp = request.OwnerUserIcp,
                Mandant = (int)request.Mandant,
                Risk_business_case_id = request.RiskBusinessCaseId
            }
        };

        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + "/wfs/eventreport/casestatechanged", easRequest, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Contracts.WFS_Event_Response>(cancellationToken: cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, StartupExtensions.ServiceName, nameof(CaseStateChanged), nameof(Contracts.WFS_Event_Response));

            // neco je spatne ve WS
            if ((result.Result?.Return_val ?? 0) != 0)
                throw new CisExtServiceValidationException($"{StartupExtensions.ServiceName}.CaseStateChanged: {result.Result?.Return_text}");
        }
        else
        {
            throw new CisExtServiceValidationException($"{StartupExtensions.ServiceName} unknown error {response.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}");
        }
    }

    private readonly HttpClient _httpClient;
    public RealSbWebApiClient(HttpClient httpClient)
        => _httpClient = httpClient;
}
