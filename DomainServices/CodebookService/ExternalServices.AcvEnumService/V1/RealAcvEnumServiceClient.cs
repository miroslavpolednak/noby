using CIS.Core;
using CIS.Infrastructure.ExternalServicesHelpers;
using System.ComponentModel.DataAnnotations;

namespace DomainServices.CodebookService.ExternalServices.AcvEnumService.V1;

internal sealed class RealAcvEnumServiceClient
    : IAcvEnumServiceClient
{
    public async Task<List<Contracts.EnumItemDTO>> GetCategory(Categories category, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(_httpClient.BaseAddress + $"/data/category/{category.GetAttribute<DisplayAttribute>()!.Name}", cancellationToken)
            .ConfigureAwait(false);

        var acvResponse = await response.EnsureSuccessStatusAndReadJson<List<Contracts.EnumItemDTO>>(StartupExtensions.ServiceName, cancellationToken);
        return acvResponse;
    }

    private readonly HttpClient _httpClient;

    public RealAcvEnumServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
