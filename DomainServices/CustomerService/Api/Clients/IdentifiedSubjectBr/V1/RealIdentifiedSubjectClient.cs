namespace DomainServices.CustomerService.Api.Clients.IdentifiedSubjectBr.V1;

internal class RealIdentifiedSubjectClient : BaseClient<ApiException<Error>>, IIdentifiedSubjectClient
{
    public RealIdentifiedSubjectClient(HttpClient httpClient, ILogger logger) : base(httpClient, logger)
    {
    }

    public Task<object> CreateIdentifiedSubject(CreateCustomerRequest request, string traceId, CancellationToken cancellationToken)
    {
        return CallEndpoint(CreateIdentifiedSubject);

        async Task<object> CreateIdentifiedSubject()
        {
            //var result = await CreateClient().CreateIdentifiedSubjectAsync();

            return null;
        }
    }

    private IdentifiedSubjectBrWrapper CreateClient() => new(_httpClient) { BaseUrl = _httpClient.BaseAddress?.ToString() };

    protected override int GetApiExceptionStatusCode(ApiException<Error> ex) => ex.StatusCode;

    protected override object GetApiExceptionDetail(ApiException<Error> ex) => ex.Result.Detail;
}