using CIS.Infrastructure.Logging;

namespace DomainServices.CustomerService.Api.Clients.IdentifiedSubjectBr.V1;

internal class RealIdentifiedSubjectClient : BaseClient<ApiException<Error>>, IIdentifiedSubjectClient
{
    public RealIdentifiedSubjectClient(HttpClient httpClient, ILogger logger) : base(httpClient, logger)
    {
    }

    public Task<CreateIdentifiedSubjectResponse> CreateIdentifiedSubject(IdentifiedSubject request, bool hardCreate, string traceId, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Run inputs: IdentifiedSubject CreateIdentifiedSubject with data {request}", request);

        return CallEndpoint(CreateSubject);

        async Task<CreateIdentifiedSubjectResponse> CreateSubject()
        {
            var result = await CreateClient().CreateIdentifiedSubjectAsync(
                body: request,
                hardCreate: hardCreate,
                x_B3_TraceId: traceId,
                x_KB_Party_Identity_In_Service: string.Empty,
                x_KB_Orig_System_Identity: CallerSys,
                x_KB_Caller_System_Identity: CallerSys,
                cancellationToken);

            _logger.LogSerializedObject("CreateIdentifiedSubjectResponse", result);

            return result;
        }
    }

    private IdentifiedSubjectBrWrapper CreateClient() => new(_httpClient) { BaseUrl = _httpClient.BaseAddress?.ToString() };

    protected override int GetApiExceptionStatusCode(ApiException<Error> ex) => ex.StatusCode;

    protected override object GetApiExceptionDetail(ApiException<Error> ex) => ex.Result.Detail;
}