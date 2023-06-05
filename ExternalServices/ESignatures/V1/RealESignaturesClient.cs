using CIS.Foms.Enums;

namespace ExternalServices.ESignatures.V1;

internal sealed class RealESignaturesClient
    : IESignaturesClient
{
    public async Task<string> GetDocumentStatus(string documentId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(_httpClient.BaseAddress + $"/{_organization}/REST/v2/DocumentService/GetDocumentStatus?id={documentId}", cancellationToken)
            .ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync<Contracts.ResponseStatus>(cancellationToken: cancellationToken)
            ?? throw new CisExtServiceResponseDeserializationException(0, StartupExtensions.ServiceName, nameof(GetDocumentStatus), nameof(Contracts.ResponseStatus));
        return result.Status!;
    }

    public Task DownloadDocumentPreview(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task SubmitDispatchForm(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private const string _organization = "mpss";

    private readonly HttpClient _httpClient;
    public RealESignaturesClient(HttpClient httpClient)
        => _httpClient = httpClient;
}