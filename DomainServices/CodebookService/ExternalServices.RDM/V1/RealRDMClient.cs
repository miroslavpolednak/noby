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

    public async Task<List<Contracts.GetCodebookMappingResponse_CodebookEntryMapping>> GetMappingItems(string codebookCode, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(_httpClient.BaseAddress + $"/codebook-mappings/{codebookCode}", cancellationToken)
            .ConfigureAwait(false);

        var cbResponse = await response.EnsureSuccessStatusAndReadJson<Contracts.GetCodebookMappingResponse_GetCodebookMappingResponse>(StartupExtensions.ServiceName, cancellationToken);
        return cbResponse.CodebookMapping.CodebookEntryMappings.ToList();
    }

    private readonly HttpClient _httpClient;

    public RealRDMClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
