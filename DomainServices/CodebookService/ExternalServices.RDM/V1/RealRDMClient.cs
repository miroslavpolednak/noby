using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.CodebookService.ExternalServices.RDM.V1;

internal sealed class RealRDMClient
    : IRDMClient
{
    public async Task<List<Contracts.GetCodebookResponse_CodebookEntry>> GetCodebookItems(string codebookCode, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(_httpClient.BaseAddress + $"/codebooks/{codebookCode}", cancellationToken)
            .ConfigureAwait(false);

        var cbResponse = await response.EnsureSuccessStatusAndReadJson<Contracts.GetCodebookResponse_GetCodebookResponse>(StartupExtensions.ServiceName, cancellationToken);
        return cbResponse.Codebook.CodebookEntries.ToList();
    }

    private readonly HttpClient _httpClient;

    public RealRDMClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
