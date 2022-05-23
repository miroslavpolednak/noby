using ExternalServices.SbWebApi.Shared;
using ExternalServices.SbWebApi.Shared.Responses;

namespace ExternalServices.SbWebApi.V1.Clients;

internal class RealSbWebApiClient
    : ISbWebApiClient
{
    private readonly HttpClient _client;
    private readonly ILogger<RealSbWebApiClient> _logger;

    const string _caseStateChangedUrl = "/wfs/eventreport/casestatechanged";

    public RealSbWebApiClient(HttpClient client, ILogger<RealSbWebApiClient> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<IServiceCallResult> CaseStateChanged(CaseStateChangedModel request, CancellationToken cancellationToken)
    {
        var transformedRequest = (Shared.Requests.CaseStateChangedRequest)request;
        _logger.ExtServiceRequest(StartupExtensions.ServiceName, _caseStateChangedUrl, transformedRequest);
        
        var response = await _client.PostAsJsonAsync(_client.BaseAddress + _caseStateChangedUrl, transformedRequest, cancellationToken);
        var result = await response.Content.ReadFromJsonAsync<CaseStateChangedResponse>() 
            ?? throw new CisExtServiceResponseDeserializationException(0, StartupExtensions.ServiceName, _caseStateChangedUrl, nameof(CaseStateChangedResponse));
        
        _logger.ExtServiceResponse(StartupExtensions.ServiceName, _caseStateChangedUrl, result);

        if (result.result.return_val == 0)
            return new SuccessfulServiceCallResult();
        else
            return new ErrorServiceCallResult(result.result.return_val, result.result.return_text ?? "");
    }
}
