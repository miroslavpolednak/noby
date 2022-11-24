using ExternalServices.SbWebApi.Shared;
using ExternalServices.SbWebApi.V1.SbWebApiWrapper;

namespace ExternalServices.SbWebApi.V1;

internal class RealSbWebApiClient
    : BaseClient<EventReportClient>, ISbWebApiClient
{
    public RealSbWebApiClient(HttpClient httpClient, ILogger<EventReportClient> logger) : base(httpClient, logger) { }

    public async Task<IServiceCallResult> CaseStateChanged(CaseStateChangedModel request, CancellationToken cancellationToken)
    {
        return await WithClient(async c => {

            return await callMethod(async () => {
                var easRequest = new WFS_Request_CaseStateChanged
                {
                    Header = new WFS_Header
                    {
                        Systemx = "NOBY",
                        Login = request.Login
                    },
                    Message = new WFS_Event_CaseStateChanged
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
                _logger.LogSerializedObject("CaseStateChanged", easRequest);

                var result = await c.CaseStateChangedAsync(easRequest);

                if (result.Result.Return_val.GetValueOrDefault() == 0)
                    return new SuccessfulServiceCallResult();

                return new ErrorServiceCallResult(9602,
                    $"An error occurred when calling the RIP service (CaseStateChanged) – {result.Result.Return_val.GetValueOrDefault()}: {result.Result.Return_text ?? ""}");
            });

        });
    }

    private EventReportClient CreateClient()
        => new(_httpClient?.BaseAddress?.ToString(), _httpClient);

    private async Task<IServiceCallResult> WithClient(Func<EventReportClient, Task<IServiceCallResult>> fce)
    {
        try
        {
            return await fce(CreateClient());
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ErrorServiceCallResult(9603, $"Error occured during call external service RIP [{ex.Message}]");
        }
    }
}
