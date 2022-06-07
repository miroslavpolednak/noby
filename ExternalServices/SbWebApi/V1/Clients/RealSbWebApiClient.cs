using ExternalServices.SbWebApi.Shared;
using ExternalServices.SbWebApi.V1.SbWebApiWrapper;

namespace ExternalServices.SbWebApi.V1;

internal class RealSbWebApiClient
    : BaseClient<EventReportClient>, ISbWebApiClient
{
    public RealSbWebApiClient(HttpClient httpClient, ILogger<EventReportClient> logger) : base(httpClient, logger) { }

    public async Task<IServiceCallResult> CaseStateChanged(CaseStateChangedModel request, CancellationToken cancellationToken)
    {
        _logger.LogSerializedObject("CaseStateChanged", request);

        return await WithClient(async c => {

            return await callMethod(async () => {

                var result = await c.CaseStateChangedAsync(new WFS_Request_CaseStateChanged 
                {
                    Header = new WFS_Header
                    {
                        System = "NOBY",
                        Login = request.Login
                    },
                    Message = new WFS_Event_CaseStateChanged
                    {
                        Case_id = request.CaseId,
                        Uver_id = request.CaseId,
                        Contract_no = request.ContractNumber,
                        Jmeno_prijmeni = request.ClientFullName,
                        Case_state = request.CaseStateName,
                        Product_type = request.ProductTypeId,
                        Owner_cpm = request.OwnerUserCpm,
                        Owner_icp = request.OwnerUserIcp,
                        Mandant = (int)request.Mandant,
                        Risk_business_case_id = request.RiskBusinessCaseId
                    }
                });

                if (result.Result.Return_val.GetValueOrDefault() == 0)
                    return new SuccessfulServiceCallResult();
                else
                    return new ErrorServiceCallResult(result.Result.Return_val.GetValueOrDefault(), result.Result.Return_text ?? "");
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
            return new ErrorServiceCallResult(0, $"Error occured during call external service RIP [{ex.Message}]");
        }
    }
}
