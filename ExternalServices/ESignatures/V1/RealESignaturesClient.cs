using CIS.Foms.Enums;

namespace ExternalServices.ESignatures.V1;

internal sealed class RealESignaturesClient
    : IESignaturesClient
{
    public async Task<string?> GetDocumentStatus(string documentId, IdentitySchemes mandant, CancellationToken cancellationToken = default(CancellationToken))
    {
        string org = "mpss"; //TODO zatim zname jen mpss?

        var response = await _httpClient
            .GetAsync(_httpClient.BaseAddress + $"/{org}/REST/DocumentService/GetDocumentStatus?id={documentId}", cancellationToken)
            .ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Contracts.ResponseStatus>(cancellationToken: cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, StartupExtensions.ServiceName, nameof(GetDocumentStatus), nameof(Contracts.ResponseStatus));
            return result.Status;
        }
        else
        {
            return null;
            // TODO muze to vracet 400?
        }
    }

    private readonly HttpClient _httpClient;
    public RealESignaturesClient(HttpClient httpClient)
        => _httpClient = httpClient;
}