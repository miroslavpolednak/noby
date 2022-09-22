using CIS.Infrastructure.Logging;

namespace DomainServices.CustomerService.Api.Clients.IdentifiedSubjectBr.V1;

internal class RealIdentifiedSubjectClient : BaseClient<IdentifiedSubjectBrWrapper>, IIdentifiedSubjectClient
{
    public RealIdentifiedSubjectClient(HttpClient httpClient, ILogger<RealIdentifiedSubjectClient> logger) : base(httpClient, logger)
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

    protected override IdentifiedSubjectBrWrapper CreateClient() => new(_httpClient) { BaseUrl = $"{_httpClient.BaseAddress}/public" };
}