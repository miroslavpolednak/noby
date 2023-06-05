using CIS.Foms.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;

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

        //TODO osetrit chybove stavy???
        if ((result.Result?.Code ?? 0) == 0)
        {
            return result.Status!;
        }
        else 
        {
            throw new CisExtServiceValidationException(result.Result!.Message ?? "");
        }
        
    }

    public Task DownloadDocumentPreview(string externalId, CancellationToken cancellationToken = default)
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