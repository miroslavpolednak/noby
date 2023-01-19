using CIS.InternalServices.DataAggregatorService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Clients.Services;

internal class DataAggregatorServiceClient : IDataAggregatorServiceClient
{
    private readonly Contracts.V1.DataAggregatorService.DataAggregatorServiceClient _client;

    public DataAggregatorServiceClient(Contracts.V1.DataAggregatorService.DataAggregatorServiceClient client)
    {
        _client = client;
    }

    public Task<GetDocumentDataResponse> GetDocumentData(GetDocumentDataRequest request, CancellationToken cancellationToken = default) =>
        _client.GetDocumentDataAsync(request, cancellationToken: cancellationToken).ResponseAsync;

    public Task<GetEasFormResponse> GetEasForm(GetEasFormRequest request, CancellationToken cancellationToken = default) => 
        _client.GetEasFormAsync(request, cancellationToken: cancellationToken).ResponseAsync;
}