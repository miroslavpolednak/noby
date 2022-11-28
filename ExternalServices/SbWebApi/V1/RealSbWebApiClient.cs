using ExternalServices.SbWebApi.Dto;

namespace ExternalServices.SbWebApi.V1;

internal sealed class RealSbWebApiClient
    : ISbWebApiClient
{
    public async Task<IServiceCallResult> CaseStateChanged(CaseStateChangedRequest request, CancellationToken cancellationToken)
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
            .PutAsJsonAsync(_httpClient.BaseAddress + "", cancellationToken)
            .ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync<CreditWorthinessCalculation>(HttpClientFactoryExtensions.CustomJsonOptions, cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(17001, CreditWorthinessStartupExtensions.ServiceName, nameof(Calculate), nameof(CreditWorthinessCalculation));
    }

    private readonly HttpClient _httpClient;

    public RealSbWebApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
