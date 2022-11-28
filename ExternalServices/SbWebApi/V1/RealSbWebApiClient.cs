using ExternalServices.SbWebApi.Dto;

namespace ExternalServices.SbWebApi.V1;

internal sealed class RealSbWebApiClient
    : ISbWebApiClient
{
    public async Task CaseStateChanged(CaseStateChangedRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
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
            .PostAsJsonAsync("wfs/eventreport/casestatechanged", easRequest, cancellationToken)
            .ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync<Contracts.WFS_Event_Response>(cancellationToken: cancellationToken)
            ?? throw new CisExtServiceResponseDeserializationException(0, StartupExtensions.ServiceName, nameof(CaseStateChanged), nameof(Contracts.WFS_Event_Response));

        // neco je spatne ve WS
        if ((result.Result?.Return_val ?? 0) != 0)
            throw new CisExtServiceValidationException(result.Result?.Return_text ?? "");
    }

    private readonly HttpClient _httpClient;

    public RealSbWebApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
