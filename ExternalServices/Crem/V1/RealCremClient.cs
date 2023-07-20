using CIS.Infrastructure.ExternalServicesHelpers;

namespace ExternalServices.Crem.V1;

internal sealed class RealCremClient
    : ICremClient
{
    public async Task<Dto.FlatsForAddress> GetFlatsForAddress(long addressPointId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(_httpClient.BaseAddress + $"/deed-of-ownership-document/addresses/{addressPointId}/flats", cancellationToken)
            .ConfigureAwait(false);

        var result = await response.EnsureSuccessStatusAndReadJson<Contracts.ResponseGetFlatsForAddressDTO>(StartupExtensions.ServiceName, cancellationToken);

        return new Dto.FlatsForAddress
        {
            DeedOfOwnershipId = result.Building.IsknDeedOfOwnershipId,
            Flats = result.Building.Flats.Select(t => new Dto.FlatsForAddress.Flat
            {
                DeedOfOwnershipId = t.IsknDeedOfOwnershipId,
                FlatNumber = t.FlatNumber
            }).ToList()
        };
    }

    private readonly HttpClient _httpClient;

    public RealCremClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
