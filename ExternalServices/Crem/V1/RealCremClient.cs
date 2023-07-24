using CIS.Infrastructure.ExternalServicesHelpers;

namespace ExternalServices.Crem.V1;

internal sealed class RealCremClient
    : ICremClient
{
    public async Task<Contracts.ResponseGetFlatsForAddressDTO> GetFlatsForAddress(long addressPointId, CancellationToken cancellationToken = default)
    {
        return await (await _httpClient
            .GetAsync(_httpClient.BaseAddress + $"/deed-of-ownership-document/addresses/{addressPointId}/flats", cancellationToken)
            .ConfigureAwait(false))
            .EnsureSuccessStatusAndReadJson<Contracts.ResponseGetFlatsForAddressDTO>(StartupExtensions.ServiceName, cancellationToken);
    }

    public async Task<ICollection<Contracts.DeedOfOwnershipDocument>> GetDocuments(int? katuzId, int? deedOfOwnershipNumber, long? deedOfOwnershipId, CancellationToken cancellationToken = default)
    {
        var result = await (await _httpClient
            .GetAsync(_httpClient.BaseAddress + $"/deed-of-ownership-document?territoryNumber={katuzId}&deedOfOwnershipNumber={deedOfOwnershipNumber}&deedOfOwnershipIsknId={deedOfOwnershipId}", cancellationToken)
            .ConfigureAwait(false))
            .EnsureSuccessStatusAndReadJson<Contracts.ResponseSearchDeedOfOwnershipDocuments>(StartupExtensions.ServiceName, cancellationToken);
        return result.Items;
    }

    public async Task<long> RequestNewDocumentId(int? katuzId, int? deedOfOwnershipNumber, long? deedOfOwnershipId, CancellationToken cancellationToken = default)
    {
        var request1 = new Contracts.RequestDownloadDeedOfOwnership
        {
            DeedOfOwnershipNumber = deedOfOwnershipNumber,
            IsknDeedOfOwnershipId = deedOfOwnershipId,
            TerritoryNumber = katuzId
        };

        var result1 = await (await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + "/deed-of-ownership-document", request1, cancellationToken)
            .ConfigureAwait(false))
            .EnsureSuccessStatusAndReadJson<Contracts.DeedOfOwnershipDocument>(StartupExtensions.ServiceName, cancellationToken);

        for (int i = 1; i <= _requestNewDocumentIterations; i++)
        {
            var result2 = await (await _httpClient
                .GetAsync(_httpClient.BaseAddress + $"/deed-of-ownership-document/{result1.DocumentId}/status", cancellationToken)
                .ConfigureAwait(false))
                .EnsureSuccessStatusAndReadJson<Contracts.DocumentStatus>(StartupExtensions.ServiceName, cancellationToken);

            // pokud je dokument v pozadovanem statusu, muzeme pokracovat dale
            if (result2.StatusCode == Contracts.DocumentStatusStatusCode.WAITING_FOR_PRICE_CONFIRMATION)
            {
                break;
            }

            // jestlize ani v posledni iteraci neprisel pozadovany status, je to maler
            if (i == _requestNewDocumentIterations)
            {
                throw new CisExtServiceValidationException("Can not obtaion WAITING_FOR_PRICE_CONFIRMATION document status");
            }

            Thread.Sleep(_requestNewDocumentTimeout * i);
        }
        

        await (await _httpClient
            .PutAsync(_httpClient.BaseAddress + $"/deed-of-ownership-document/{result1.DocumentId}", null, cancellationToken)
            .ConfigureAwait(false))
            .EnsureSuccessStatusCode(StartupExtensions.ServiceName, cancellationToken);

        return result1.DocumentId;
    }

    public async Task<ICollection<Contracts.DeedOfOwnershipOwnerDTO>> GetOwners(long documentId, CancellationToken cancellationToken = default)
    {
        var result = await (await _httpClient
            .GetAsync(_httpClient.BaseAddress + $"/deed-of-ownership-document/{documentId}/owners", cancellationToken)
            .ConfigureAwait(false))
            .EnsureSuccessStatusAndReadJson<Contracts.ResponseGetOwners>(StartupExtensions.ServiceName, cancellationToken);
        return result.Items;
    }

    public async Task<ICollection<Contracts.DeedOfOwnershipLegalRelation>> GetLegalRelations(long documentId, CancellationToken cancellationToken = default)
    {
        var result = await (await _httpClient
            .GetAsync(_httpClient.BaseAddress + $"/deed-of-ownership-document/{documentId}/legal-relations", cancellationToken)
            .ConfigureAwait(false))
            .EnsureSuccessStatusAndReadJson<Contracts.ResponseGetLegalRelations>(StartupExtensions.ServiceName, cancellationToken);
        return result.Items;
    }

    public async Task<ICollection<Contracts.DeedOfOwnershipRealEstate>> GetRealEstates(long documentId, CancellationToken cancellationToken = default)
    {
        var result = await (await _httpClient
            .GetAsync(_httpClient.BaseAddress + $"/deed-of-ownership-document/{documentId}/real-estates", cancellationToken)
            .ConfigureAwait(false))
            .EnsureSuccessStatusAndReadJson<Contracts.ResponseGetRealEstates>(StartupExtensions.ServiceName, cancellationToken);
        return result.Items;
    }

    private const int _requestNewDocumentIterations = 10;
    private const int _requestNewDocumentTimeout = 1000;
    private readonly HttpClient _httpClient;

    public RealCremClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
